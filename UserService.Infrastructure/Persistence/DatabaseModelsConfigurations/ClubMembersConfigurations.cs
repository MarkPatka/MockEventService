using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.ClubAggregate;
using UserService.Domain.ClubAggregate.ValueObjects;
using UserService.Domain.UserProfileAggregate.ValueObjects;

namespace UserService.Infrastructure.Persistence.DatabaseModelsConfigurations;

public class ClubMembersConfigurations : IEntityTypeConfiguration<ClubMember>
{
    public void Configure(EntityTypeBuilder<ClubMember> builder)
    {
        builder.ToTable("ClubToMembers");

        builder.Property(x => x.ClubId).HasConversion(
            id => id.Value,
            value => ClubId.Create(value)).IsRequired();

        builder.Property(x => x.UserId).HasConversion(
            id => id.Value,
            value => UserId.Create(value)).IsRequired();

        builder.Property(e => e.JoinedAt).HasColumnType("timestamp with time zone").IsRequired();
    }
}