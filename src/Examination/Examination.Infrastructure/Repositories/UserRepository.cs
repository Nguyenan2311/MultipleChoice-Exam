using Examination.Infrastructure.SeedWork;
using Exemination.Domain.AggregateModels.UserAggregate;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Examination.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(IMongoClient mongoClient, IOptions<ExamSettings> settings, string collection) : base(mongoClient, settings, collection)
    {
    }

    public async Task<User> GetUserByIdAsync(string externalId)
    {
        var filter = Builders<User>.Filter.Eq(s => s.ExternalId, externalId);
            return await Collection.Find(filter).FirstOrDefaultAsync();
    }
}
