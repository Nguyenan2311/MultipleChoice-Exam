namespace Exemination.Domain.AggregateModels.UserAggregate;

public interface IUserRepository
{
    Task<User> GetUserByIdAsync(string externalId);
}
