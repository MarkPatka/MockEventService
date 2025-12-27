using MediatR;
using MockEventService.Application.EventManagement.Common;

namespace MockEventService.Application.EventManagement.Queries.GetAllUserEventsQuery;

public record GetAllUserEventsQuery(string userId) : IRequest<GetAllUserEventsResult>;
