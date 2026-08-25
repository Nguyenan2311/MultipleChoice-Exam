namespace Exemination.Domain.AggregateModels.SeedWork;

public interface IUnitOfWork : IDisposable
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
