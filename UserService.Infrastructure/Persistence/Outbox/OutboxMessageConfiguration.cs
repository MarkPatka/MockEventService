using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserService.Infrastructure.Persistence.Outbox;

public class OutboxMessageConfiguration 
    : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_messages");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Type)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Payload)
            .IsRequired();

        builder.Property(x => x.AggregateType)
            .HasMaxLength(200);

        builder.Property(x => x.AggregateId)
            .HasMaxLength(100);

        builder.HasIndex(x => x.ProcessedOnUtc);
    }
}
