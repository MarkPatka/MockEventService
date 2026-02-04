using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MockEventService.Domain.EventAggregate.Enumerations;

namespace MockEventService.Infrastructure.Persistence.EntityTypesConfigurations;

public class ParticipantStatusConfiguration : IEntityTypeConfiguration<ParticipantStatus>
{
    public void Configure(EntityTypeBuilder<ParticipantStatus> builder)
    {
        builder.ToTable("ParticipantStatuses");

        builder.HasKey(ps => ps.Id);

        builder.Property(ps => ps.Id)
            .HasColumnName("ParticipantStatusId")
            .ValueGeneratedNever();

        builder.Property(ps => ps.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasData(ParticipantStatus.List);
    }
}

