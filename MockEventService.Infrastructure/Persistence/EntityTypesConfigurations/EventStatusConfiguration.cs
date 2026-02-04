using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MockEventService.Domain.EventAggregate.Enumerations;

namespace MockEventService.Infrastructure.Persistence.EntityTypesConfigurations;

public class EventStatusConfiguration : IEntityTypeConfiguration<EventStatus>
{
    public void Configure(EntityTypeBuilder<EventStatus> builder)
    {
        builder.ToTable("EventStatuses");

        builder.HasKey(es => es.Id);

        builder.Property(es => es.Id)
            .HasColumnName("EventStatusId")
            .ValueGeneratedNever();

        builder.Property(es => es.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasData(EventStatus.List);
    }
}