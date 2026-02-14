# Event Sourcing Pipeline — MockEventService

## Table of Contents

1. [Architecture Overview](#1-architecture-overview)
2. [Full Pipeline: Step-by-Step](#2-full-pipeline-step-by-step)
3. [Layer-by-Layer Breakdown](#3-layer-by-layer-breakdown)
4. [Code Review: Issues Found](#4-code-review-issues-found)
5. [What Works Correctly](#5-what-works-correctly)
6. [Presentation Conspectus](#6-presentation-conspectus)

---

## 1. Architecture Overview

The project follows **Clean Architecture** with **Domain-Driven Design (DDD)** and **CQRS** patterns. Event sourcing is implemented via **domain events** dispatched through **MediatR** and published to **Apache Kafka** as **integration events**.

```
┌─────────────────────────────────────────────────────────────────┐
│                        API Layer                                │
│  Controller → MediatR Command → CommandHandler                  │
└──────────────────────────┬──────────────────────────────────────┘
                           │
┌──────────────────────────▼──────────────────────────────────────┐
│                    Application Layer                             │
│  Commands/Queries, IUnitOfWork, IEventProducer, IEventConsumer  │
│  IntegrationEvent base, KafkaOptions                            │
└──────────────────────────┬──────────────────────────────────────┘
                           │
┌──────────────────────────▼──────────────────────────────────────┐
│                   Infrastructure Layer                           │
│  UnitOfWork (dispatch), EventHandlers, KafkaProducer,           │
│  KafkaConsumer, DeadLetterQueue, EF Core DbContext              │
└──────────────────────────┬──────────────────────────────────────┘
                           │
┌──────────────────────────▼──────────────────────────────────────┐
│                      Domain Layer                                │
│  AggregateRoot, Entity, IDomainEvent, IEventSourceable          │
│  Event aggregate, Domain Events, Value Objects                   │
└─────────────────────────────────────────────────────────────────┘
```

---

## 2. Full Pipeline: Step-by-Step

Below is the complete lifecycle of an event from API request to Kafka topic, using **Event Creation** as the example.

### Step 1 — API Request

The client sends a POST request to the `EventsController`. The controller maps the request DTO to a `CreateEventCommand` and sends it via MediatR.

```
HTTP POST /api/events  →  CreateEventCommand  →  MediatR.Send()
```

### Step 2 — Command Handler

`CreateEventCommandHandler` receives the command:

1. Checks if the event already exists via `IEventService.CheckEventNotExists()`
2. Calls the **domain factory method** `Event.Create(...)` which:
   - Constructs a new `Event` aggregate root with a unique `EventId`
   - **Raises a domain event**: `AddDomainEvent(new EventCreated(...))`
3. Calls `IEventService.CreateEventAsync()` to persist via the repository
4. Calls `IUnitOfWork.SaveEntitiesAsync()` to save and dispatch events

```
CreateEventCommandHandler.Handle()
  │
  ├── Event.Create(...)              ← Domain event raised here
  │     └── AddDomainEvent(new EventCreated(eventId, now))
  │
  ├── _eventService.CreateEventAsync(event)
  │     ├── _repository.AddAsync(event)
  │     └── _unitOfWork.SaveChangesAsync()   ← Events dispatched + DB save
  │
  └── _unitOfWork.SaveEntitiesAsync()        ← Second save (redundant, see review)
```

### Step 3 — Domain Event Dispatch (UnitOfWork)

Inside `UnitOfWork.SaveChangesAsync()`, **before** the database save:

1. `DispatchDomainEventsAsync()` queries the EF Core **ChangeTracker** for all entities implementing `IEventSourceable` that have pending domain events
2. Collects all `IDomainEvent` instances from those entities
3. **Clears** the domain events from the entities (prevents infinite loop if handlers modify entities)
4. Publishes each domain event via `MediatR IPublisher.Publish()` — which dispatches to all registered `INotificationHandler<T>` implementations

```
UnitOfWork.SaveChangesAsync()
  │
  ├── DispatchDomainEventsAsync()
  │     ├── ChangeTracker.Entries<IEventSourceable>()
  │     │     └── Filter: entities with DomainEvents.Count != 0
  │     ├── Collect all IDomainEvent from entities
  │     ├── entity.ClearDomainEvents()       ← Clear BEFORE publish
  │     └── foreach event:
  │           └── _publisher.Publish(domainEvent)   ← MediatR dispatch
  │
  └── _context.SaveChangesAsync()            ← EF Core DB save
```

### Step 4 — Domain Event Handler (Domain → Integration Event Conversion)

MediatR routes the `EventCreated` domain event to `EventCreatedDomainEventHandler` (registered as `INotificationHandler<EventCreated>` in Infrastructure DI).

The handler:

1. Converts the **domain event** into an **integration event** — mapping value objects (`EventId`) to primitives (`Guid`) for serialization
2. Calls `IEventProducer.ProduceAsync()` to publish to Kafka

```
EventCreatedDomainEventHandler.Handle(EventCreated notification)
  │
  ├── new EventCreatedIntegrationEvent {
  │       EventId   = notification.EventId.Value,    // Guid
  │       CreatedAt = notification.CreatedAt
  │   }
  │
  └── _eventProducer.ProduceAsync(
          topic: _options.ProduceEventsTopic,
          key:   eventId.ToString(),
          message: integrationEvent)
```

**Key concept**: Domain events use domain types (value objects). Integration events use primitives (Guid, DateTime) — safe for JSON serialization and cross-service communication.

### Step 5 — Kafka Producer

`KafkaEventProducer.ProduceAsync()`:

1. Serializes the integration event to **JSON** (camelCase)
2. Produces to the configured Kafka topic via `Confluent.Kafka` `IProducer<string, string>`
3. On failure: attempts to send the message to the **Dead Letter Queue (DLQ)** topic, then re-throws

```
KafkaEventProducer.ProduceAsync()
  │
  ├── JsonSerializer.Serialize(message)      ← JSON payload
  │
  ├── _producer.ProduceAsync(topic, message) ← Kafka send
  │     Config: Acks=All, Idempotence=true
  │
  └── On ProduceException:
        ├── SendToDeadLetterQueueAsync()     ← DLQ fallback
        └── throw                            ← Re-throw to caller
```

### Step 6 — Message on Kafka Topic

The integration event is now on the Kafka topic as a JSON message:

```json
{
  "id": "a1b2c3d4-...",
  "occurredOn": "2026-02-14T10:30:00Z",
  "eventType": "EventCreatedIntegrationEvent",
  "eventId": "e5f6g7h8-...",
  "createdAt": "2026-02-14T10:30:00Z"
}
```

The Kafka message key is the `EventId` (Guid string), ensuring all events for the same aggregate go to the same partition (ordering guarantee per aggregate).

### Step 7 — Consumer Side (Not Fully Implemented)

`KafkaEventConsumer` (extends `BackgroundService`) would:

1. Subscribe to configured topics
2. Consume messages in a loop with manual offset commit
3. Deserialize the JSON into a `KafkaEventEnvelope`
4. Map the envelope to a CQRS command via `MapToCommand()` (currently `NotImplementedException`)
5. Send the command via MediatR `ISender`
6. On failure: retry with linear backoff, then route to DLQ after max retries

```
KafkaEventConsumer.ExecuteAsync()
  │
  ├── BuildConsumer() → Subscribe(topics)
  │
  └── ConsumeMessagesAsync() loop:
        ├── _consumer.Consume()
        ├── TryProcessMessageWithRetryAsync()
        │     ├── ProcessMessageAsync()
        │     │     ├── Deserialize → KafkaEventEnvelope
        │     │     ├── MapToCommand(envelope)        ← NOT IMPLEMENTED
        │     │     └── mediator.Send(command)
        │     │
        │     └── On failure (after max retries):
        │           └── _dlqProducer.PublishAsync()
        │
        └── _consumer.Commit() on success
```

---

## 3. Layer-by-Layer Breakdown

### Domain Layer

| Component | File | Responsibility |
|-----------|------|----------------|
| `AggregateRoot<Tid>` | `Domain/Common/Abstract/AggregateRoot.cs` | Base class managing `_domainEvents` list. Methods: `AddDomainEvent`, `RemoveDomainEvent`, `ClearDomainEvents` |
| `IDomainEvent` | `Domain/Common/Abstract/IDomainEvent.cs` | Marker interface extending MediatR `INotification`. Property: `OccurredOn` |
| `IEventSourceable` | `Domain/Common/Abstract/IEventSourceable.cs` | Interface for entities that produce domain events. Used by UnitOfWork to find events in ChangeTracker |
| `Event` | `Domain/EventAggregate/Event.cs` | Aggregate root. Factory method `Create()` and behavior methods `Publish()`, `RegisterParticipant()`, `Cancel()` each raise domain events |
| `EventCreated` | `Domain/EventAggregate/DomainEvents/EventCreated.cs` | Sealed record. Raised in `Event.Create()` |
| `EventPublished` | `Domain/EventAggregate/DomainEvents/EventPublished.cs` | Sealed record. Raised in `Event.Publish()` |
| `ParticipantRegistered` | `Domain/EventAggregate/DomainEvents/ParticipantRegistered.cs` | Sealed record. Raised in `Event.RegisterParticipant()` |
| `EventCancelled` | `Domain/EventAggregate/DomainEvents/EventCancelled.cs` | Sealed record. Raised in `Event.Cancel()` |

**How domain events are managed in AggregateRoot:**
- Private `List<IDomainEvent> _domainEvents` stores pending events
- Exposed as `IReadOnlyCollection<IDomainEvent> DomainEvents` (via `AsReadOnly()`)
- Domain methods call `AddDomainEvent(new SomeEvent(...))` to queue events
- Events stay in the list until `ClearDomainEvents()` is called by the UnitOfWork during dispatch

### Application Layer

| Component | File | Responsibility |
|-----------|------|----------------|
| `IntegrationEvent` | `Application/EventSourcing/IntegrationEvent.cs` | Abstract base record with `Id`, `OccurredOn`, `EventType`. Uses primitives for serialization |
| `IEventProducer` | `Application/EventSourcing/IEventProducer.cs` | Interface: `ProduceAsync<TMessage>(topic, key, message)`. Extends `IDisposable` |
| `IEventConsumer` | `Application/EventSourcing/IEventConsumer.cs` | Interface: `ConsumeMessagesAsync(CancellationToken)`. Extends `IDisposable` |
| `IDeadLetterQueueProducer` | `Application/EventSourcing/IDeadLetterQueueProducer.cs` | Interface: `PublishAsync(originalTopic, key, value, errorReason, exception)` |
| `KafkaOptions` | `Application/Common/Configuration/KafkaOptions.cs` | Config class: BootstrapServers, Topics, ConsumerGroupId, DLQ topic, retry settings |
| `IUnitOfWork` | `Application/Persistence/IUnitOfWork.cs` | Interface: `SaveChangesAsync`, `SaveEntitiesAsync`, transaction management |

### Infrastructure Layer

| Component | File | Responsibility |
|-----------|------|----------------|
| `UnitOfWork` | `Infrastructure/Persistence/UnitOfWork.cs` | Dispatches domain events via MediatR before saving. Transaction support. Queries ChangeTracker for `IEventSourceable` entities |
| `EventCreatedDomainEventHandler` | `Infrastructure/EventSourcing/EventHandlers/EventCreatedEventHandler.cs` | `INotificationHandler<EventCreated>`. Converts to `EventCreatedIntegrationEvent`, publishes to Kafka |
| `EventPublishedDomainEventHandler` | `Infrastructure/EventSourcing/EventHandlers/EventPublishedEventHandler.cs` | `INotificationHandler<EventPublished>`. Converts to `EventPublishedIntegrationEvent`, publishes to Kafka |
| `ParticipantRegisteredDomainEventHandler` | `Infrastructure/EventSourcing/EventHandlers/ParticipantRegisteredEventHandler.cs` | `INotificationHandler<ParticipantRegistered>`. Converts to `ParticipantRegisteredIntegrationEvent`, publishes to Kafka |
| `EventCreatedIntegrationEvent` | `Infrastructure/EventSourcing/EventContracts/EventCreatedIntegrationEvent.cs` | Sealed record extending `IntegrationEvent`. Sets `EventType = nameof(...)` |
| `EventPublishedIntegrationEvent` | `Infrastructure/EventSourcing/EventContracts/EventPublishedIntegrationEvent.cs` | Same pattern |
| `ParticipantRegisteredIntegrationEvent` | `Infrastructure/EventSourcing/EventContracts/ParticipantRegisteredIntegrationEvent.cs` | Same pattern |
| `KafkaEventProducer` | `Infrastructure/EventSourcing/Messaging/KafkaEventProducer.cs` | Implements `IEventProducer`. JSON serialization, `Acks.All`, idempotent, DLQ fallback on failure |
| `KafkaEventConsumer` | `Infrastructure/EventSourcing/Messaging/KafkaEventConsumer.cs` | Extends `BackgroundService`, implements `IEventConsumer`. Manual commit, retry, DLQ. **Not fully implemented** (`MapToCommand` throws `NotImplementedException`) |
| `DeadLetterQueueProducer` | `Infrastructure/EventSourcing/Messaging/DeadLetterQueueProducer.cs` | Implements `IDeadLetterQueueProducer`. Separate Kafka producer for DLQ. Publishes error metadata |
| `DependencyInjection` | `Infrastructure/DependencyInjection.cs` | Registers all infrastructure services: DbContext, repositories, UnitOfWork, Kafka producer/consumer, event handlers |

---

## 4. Code Review: Issues Found

### BUG 1: `IDeadLetterQueueProducer` Not Registered in DI

**File**: `Infrastructure/DependencyInjection.cs:45-56`

The `AddEventSourcing()` method registers `IEventProducer` and `IEventConsumer`, but **never registers `IDeadLetterQueueProducer`**. The `KafkaEventConsumer` constructor requires it:

```csharp
public KafkaEventConsumer(
    IServiceScopeFactory scopeFactory,
    IOptions<KafkaOptions> options,
    ILogger<KafkaEventConsumer> logger,
    IDeadLetterQueueProducer dlqProducer)  // ← This will fail to resolve
```

**Impact**: Runtime `InvalidOperationException` when the DI container tries to construct `KafkaEventConsumer`.

**Fix needed**: Add to `AddEventSourcing()`:
```csharp
services.AddSingleton<IDeadLetterQueueProducer, DeadLetterQueueProducer>();
```

---

### BUG 2: Missing `EventCancelled` Handler and Integration Event

**File**: `Domain/EventAggregate/Event.cs:133-141`

The `Cancel()` method raises `EventCancelled` domain event:
```csharp
AddDomainEvent(new EventCancelled(Id, DateTime.UtcNow));
```

But there is:
- No `EventCancelledDomainEventHandler` (no `INotificationHandler<EventCancelled>`)
- No `EventCancelledIntegrationEvent` contract
- No registration in DI

**Impact**: The `EventCancelled` domain event is dispatched by UnitOfWork via MediatR, but MediatR silently ignores notifications with no handlers. The cancellation is **never published to Kafka**. Other services will not be notified when an event is cancelled.

**Fix needed**: Create `EventCancelledIntegrationEvent`, `EventCancelledDomainEventHandler`, and register the handler.

---

### BUG 3: `KafkaEventConsumer` Not Registered as Hosted Service

**File**: `Infrastructure/DependencyInjection.cs:48`

```csharp
services.AddSingleton<IEventConsumer, KafkaEventConsumer>();
```

`KafkaEventConsumer` extends `BackgroundService`, but is registered only as `IEventConsumer`. The .NET host only auto-starts services registered as `IHostedService`. Since it is not registered via `AddHostedService<>()` or `AddSingleton<IHostedService, KafkaEventConsumer>()`, the `ExecuteAsync()` method will **never be called**.

**Impact**: The consumer will never start consuming messages. (This may be intentional since consuming is not fully implemented.)

**Fix needed (when consuming is ready)**:
```csharp
services.AddHostedService<KafkaEventConsumer>();
```

---

### ISSUE 4: Double Save in CreateEvent Flow

**Files**: `Infrastructure/Services/EventService.cs:52-58` and `Application/EventManagement/Command/CreateEventCommand/CreateEventCommandHandler.cs:46-47`

The flow executes two saves:

1. `EventService.CreateEventAsync()` calls `_unitOfWork.SaveChangesAsync()` — this dispatches domain events and saves to DB
2. `CreateEventCommandHandler` then calls `_unitOfWork.SaveEntitiesAsync()` — this calls `SaveChangesAsync()` again

On the second call, domain events are already cleared (step 1 cleared them), and there are no pending DB changes, so it is a **no-op**. The domain events are dispatched and Kafka messages are sent during step 1.

**Impact**: No data corruption, but redundant DB round-trip and potential confusion about when events are actually dispatched. If someone removes the `SaveChangesAsync` from `EventService.CreateEventAsync()`, events would still dispatch via the handler's `SaveEntitiesAsync()`. But if someone removes the handler's call, events might stop dispatching depending on which other code paths use the service.

**Recommendation**: Decide on a single responsibility — either the service saves, or the command handler saves. Not both.

---

### ISSUE 5: Domain Events Dispatched BEFORE Database Save

**File**: `Infrastructure/Persistence/UnitOfWork.cs:37-61`

```csharp
public async Task<int> SaveChangesAsync(...)
{
    await DispatchDomainEventsAsync(cancellationToken);  // Kafka messages sent HERE
    var result = await _context.SaveChangesAsync(...);    // DB save HERE
    return result;
}
```

Domain events (and consequently Kafka messages) are published **before** the database transaction commits. If `_context.SaveChangesAsync()` fails (e.g., constraint violation, connection loss), the Kafka messages have already been sent and **cannot be rolled back**.

**Impact**: Potential inconsistency — other services receive an "EventCreated" message, but the event doesn't exist in the database.

**Trade-off**: This is a known architectural decision. The alternative (dispatch after save) risks the opposite problem: DB saves but Kafka publish fails. The Outbox pattern is the typical solution for guaranteed consistency.

---

### ISSUE 6: `CheckEventNotExists` Naming Is Inverted

**File**: `Infrastructure/Services/EventService.cs:23-28`

```csharp
public async Task<bool> CheckEventNotExists(string title, UserId organizerId, ...)
{
    var spec = new GetEventByTitleAndOrganizerSpecification(title, organizerId);
    return await _eventRepository.AnyAsync(spec, cancellationToken);
    // Returns TRUE when the event EXISTS
}
```

The method is named `CheckEventNotExists` but returns `true` when the event **does** exist (via `AnyAsync`). The handler uses it correctly:

```csharp
var eventExists = await _eventService.CheckEventNotExists(...);
if (eventExists) throw new Exception("Event already exists");
```

**Impact**: No runtime bug, but the naming is confusing and error-prone for future developers.

---

### ISSUE 7: `Confluent.Kafka` Dependency in Application Layer

**File**: `Application/EventSourcing/IEventConsumer.cs:1`

```csharp
using Confluent.Kafka;  // ← Unused import, infrastructure leak
```

**File**: `Application/MockEventService.Application.csproj:10`

```xml
<PackageReference Include="Confluent.Kafka" Version="2.13.0" />
```

The Application layer references `Confluent.Kafka` as a NuGet dependency. This is a **Clean Architecture violation** — the Application layer should only define abstractions, not depend on infrastructure-specific packages. The `IEventConsumer` interface doesn't actually use any Confluent.Kafka types.

**Impact**: Architectural coupling. No runtime issue.

---

## 5. What Works Correctly

### Domain Layer — All Good

- **AggregateRoot** correctly manages domain events with proper encapsulation (`private List`, public `IReadOnlyCollection`)
- **Event aggregate** properly raises domain events in all behavior methods (`Create`, `Publish`, `RegisterParticipant`, `Cancel`)
- **Domain events** are clean sealed records implementing `IDomainEvent` with `OccurredOn` mapping
- **Business rule validation** in domain methods (status checks, capacity limits) is correctly placed inside the aggregate
- **Value Objects** ensure type safety (`EventId`, `UserId`, `ParticipantId` instead of raw Guids)

### Application Layer — All Good

- **IntegrationEvent base record** provides a clean contract for external events with primitive types
- **Interface abstractions** (`IEventProducer`, `IEventConsumer`, `IDeadLetterQueueProducer`) are properly defined in Application, keeping Infrastructure interchangeable
- **KafkaOptions** provides all configuration needed for producer and consumer
- **CQRS pattern** via MediatR is correctly wired — commands go through validation pipeline

### Infrastructure Layer — Producing Side Works

- **UnitOfWork.DispatchDomainEventsAsync()** correctly:
  - Queries ChangeTracker for `IEventSourceable` entities
  - Clears events BEFORE publishing (prevents infinite loops)
  - Publishes each event sequentially via MediatR
- **Domain Event Handlers** correctly convert domain events → integration events, mapping value objects to primitives
- **KafkaEventProducer** is well-configured:
  - `Acks.All` for durability
  - `EnableIdempotence = true` for exactly-once producer semantics
  - Proper DLQ fallback on `ProduceException`
  - JSON serialization with camelCase
  - Graceful dispose with flush
- **Integration Event Contracts** correctly set `EventType` via `nameof()` for type discrimination during deserialization
- **DeadLetterQueueProducer** uses a separate Kafka producer (good isolation), includes rich error metadata
- **KafkaEventConsumer** structure is correct — manual commit, retry with backoff, DLQ routing, scope-per-message isolation. The only incomplete part is `MapToCommand()`.

### DI Registration — Mostly Correct

- Event handlers manually registered as `INotificationHandler<T>` in Infrastructure DI — works with MediatR
- MediatR auto-scans Application assembly for command/query handlers
- DbContext correctly configured with Npgsql retry policy
- `KafkaEventProducer` as Singleton is correct (thread-safe, long-lived)

---

## 6. Presentation Conspectus

### Slide 1: What is Event Sourcing with Domain Events?

**Key points:**
- Instead of just saving the current state, we capture **what happened** as events
- Domain Events represent significant occurrences within the domain (e.g., "Event was created", "Participant registered")
- These events are published to a message broker (Kafka) so other services can react
- Two types of events: **Domain Events** (internal, rich types) and **Integration Events** (external, primitive types, JSON-serializable)

---

### Slide 2: The Domain Layer — Where Events Are Born

**Key points:**
- `AggregateRoot<Tid>` base class holds a private `List<IDomainEvent>` and exposes it as `IReadOnlyCollection`
- Each domain method that changes state also raises a domain event:
  - `Event.Create()` → raises `EventCreated`
  - `Event.Publish()` → raises `EventPublished`
  - `Event.RegisterParticipant()` → raises `ParticipantRegistered`
  - `Event.Cancel()` → raises `EventCancelled`
- Events implement `IDomainEvent : INotification` (MediatR integration)
- Events are **immutable sealed records** — once created, they cannot be modified

**Show**: `Event.Create()` method and how `AddDomainEvent(new EventCreated(...))` is called

---

### Slide 3: The Application Layer — Abstractions and Contracts

**Key points:**
- Defines **interfaces** that the Infrastructure implements: `IEventProducer`, `IEventConsumer`, `IDeadLetterQueueProducer`
- `IntegrationEvent` base record defines the contract for events sent externally (Id, OccurredOn, EventType)
- `KafkaOptions` centralizes all Kafka configuration (topics, consumer group, retry policy, DLQ)
- `IUnitOfWork` defines `SaveChangesAsync` / `SaveEntitiesAsync` — the trigger point for event dispatch
- The Application layer knows nothing about Kafka internals — only abstractions

---

### Slide 4: The UnitOfWork — The Dispatch Engine

**Key points:**
- `UnitOfWork.SaveChangesAsync()` is the **trigger** that dispatches domain events
- Before calling EF Core's `SaveChanges`, it runs `DispatchDomainEventsAsync()`:
  1. Scans the EF Core ChangeTracker for all tracked entities implementing `IEventSourceable`
  2. Collects all pending `IDomainEvent` objects
  3. Clears events from entities (prevents re-dispatch / infinite loops)
  4. Publishes each event via MediatR `IPublisher.Publish()`
- This is the **bridge** between the Domain (events raised) and Infrastructure (events handled)

**Show**: `DispatchDomainEventsAsync()` method

---

### Slide 5: Domain Event Handlers — Domain → Integration Event Translation

**Key points:**
- For each domain event type, there is a corresponding handler in Infrastructure:
  - `EventCreatedDomainEventHandler` : `INotificationHandler<EventCreated>`
  - `EventPublishedDomainEventHandler` : `INotificationHandler<EventPublished>`
  - `ParticipantRegisteredDomainEventHandler` : `INotificationHandler<ParticipantRegistered>`
- Each handler:
  1. Receives the domain event (with value objects like `EventId`)
  2. Converts it to an integration event (with primitives like `Guid`)
  3. Calls `IEventProducer.ProduceAsync()` to publish to Kafka
- This conversion layer is important: domain types stay internal, integration types are external-safe

**Show**: `EventCreatedDomainEventHandler.Handle()` method

---

### Slide 6: Kafka Producer — Reliable Message Delivery

**Key points:**
- `KafkaEventProducer` wraps Confluent.Kafka's `IProducer<string, string>`
- Configuration for reliability:
  - `Acks = All` — waits for all in-sync replicas to acknowledge
  - `EnableIdempotence = true` — prevents duplicate messages on retry
- Messages are JSON-serialized with camelCase naming
- Message key = aggregate ID (EventId) → ensures partition ordering per aggregate
- On `ProduceException`: sends failed message to **Dead Letter Queue** topic
- Graceful shutdown: flushes pending messages before dispose

**Show**: `ProduceAsync()` method and producer configuration

---

### Slide 7: Dead Letter Queue — Handling Failures

**Key points:**
- `DeadLetterQueueProducer` is a separate Kafka producer dedicated to the DLQ topic
- When a message fails to produce or consume, it's routed to the DLQ with metadata:
  - Original topic, key, and value
  - Error reason and exception details
  - Timestamp
- The DLQ allows manual inspection and replay of failed messages
- Used by both the producer (on send failure) and consumer (after max retries)

---

### Slide 8: Consumer Side (Planned Architecture)

**Key points:**
- `KafkaEventConsumer` extends `BackgroundService` — runs as a long-lived background task
- Manual offset commit (`EnableAutoCommit = false`) for at-least-once delivery guarantee
- Processing flow:
  1. Consume message from Kafka
  2. Deserialize into `KafkaEventEnvelope` (EventType, Payload, etc.)
  3. Map envelope to a CQRS command based on `EventType` (not yet implemented)
  4. Execute command via MediatR `ISender`
  5. Commit offset on success
- Retry mechanism: linear backoff (`RetryDelayMs * attempt`), max retries configurable
- After exhausting retries → route to DLQ, commit offset (don't re-consume forever)
- Each message processed in its own DI scope (`IServiceScopeFactory.CreateScope()`)

---

### Slide 9: Complete Flow Diagram

```
  ┌──────────────┐
  │  API Request  │
  └──────┬───────┘
         ▼
  ┌──────────────────────┐
  │  CreateEventCommand  │
  │  (MediatR CQRS)      │
  └──────┬───────────────┘
         ▼
  ┌──────────────────────────────┐
  │  Event.Create()              │
  │  └─ AddDomainEvent(          │
  │       EventCreated)          │
  └──────┬───────────────────────┘
         ▼
  ┌──────────────────────────────┐
  │  Repository.AddAsync()       │
  └──────┬───────────────────────┘
         ▼
  ┌──────────────────────────────┐
  │  UnitOfWork.SaveChangesAsync │
  │  ┌───────────────────────┐   │
  │  │ DispatchDomainEvents  │   │
  │  │  ├─ ChangeTracker     │   │
  │  │  ├─ Collect events    │   │
  │  │  ├─ Clear events      │   │
  │  │  └─ MediatR.Publish() │   │
  │  └───────────┬───────────┘   │
  │              ▼               │
  │  ┌───────────────────────┐   │
  │  │ EventCreated Handler  │   │
  │  │  ├─ → IntegrationEvent│   │
  │  │  └─ → Kafka Produce   │   │
  │  └───────────────────────┘   │
  │              ▼               │
  │  _context.SaveChangesAsync() │
  └──────────────┬───────────────┘
                 ▼
  ┌──────────────────────────────┐
  │  Message on Kafka Topic      │
  │  (JSON, keyed by EventId)    │
  └──────────────────────────────┘
                 ▼
  ┌──────────────────────────────┐
  │  KafkaEventConsumer          │
  │  (BackgroundService)         │
  │  ├─ Deserialize envelope     │
  │  ├─ Map to Command           │
  │  ├─ MediatR.Send(command)    │
  │  └─ Commit offset            │
  │                              │
  │  On failure → DLQ            │
  └──────────────────────────────┘
```

---

### Slide 10: Key Design Decisions and Trade-offs

| Decision | Rationale | Trade-off |
|----------|-----------|-----------|
| Dispatch events BEFORE DB save | Events are part of the same logical operation | If DB save fails, Kafka messages already sent (inconsistency risk) |
| Clear events before publish | Prevents infinite loops if handlers modify entities | If publish fails midway, some events may be lost |
| Singleton Kafka producer | Thread-safe, one TCP connection, efficient | Long-lived — must handle reconnections |
| Manual offset commit | At-least-once delivery guarantee | Consumer must be idempotent |
| DLQ for failed messages | No message loss, allows manual investigation | Requires monitoring and replay tooling |
| Integration events use primitives | Safe JSON serialization, no domain type leakage | Loses type safety at the boundary |
| MediatR as event bus | Simple in-process dispatch, handler auto-discovery | In-process only — not distributed (Kafka handles distribution) |
