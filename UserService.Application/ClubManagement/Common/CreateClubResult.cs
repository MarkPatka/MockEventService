using System;
using UserService.Domain.ClubAggregate.ValueObjects;

namespace UserService.Application.ClubManagement.Common;

public record CreateClubResult(Guid Id, string Name, string Description, Guid OwnerId, bool IsPublic);