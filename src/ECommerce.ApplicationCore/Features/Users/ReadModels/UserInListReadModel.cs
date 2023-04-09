using ECommerce.ApplicationCore.Entities;

namespace ECommerce.ApplicationCore.Features.Users.ReadModels;

public record UserInListReadModel(Guid Id, string Email, string Role, DateTime RegisteredAt)
{
    public static UserInListReadModel From(User user) => new(user.Id, user.Email, user.Role.ToString(), user.RegisteredAt);
}