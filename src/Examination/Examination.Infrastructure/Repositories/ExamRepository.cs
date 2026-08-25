using Examination.Infrastructure.SeedWork;
using Exemination.Domain.AggregateModels.ExamAggregate;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Examination.Infrastructure.Repositories;

public class ExamRepository : BaseRepository<Exam>, IExamRepository
{
    public ExamRepository(IMongoClient mongoClient, IOptions<ExamSettings> settings, string collection) : base(mongoClient, settings, collection)
    {
    }

    public async Task<IEnumerable<Exam>> GetExamListAsync()
    {
        return await Collection.Find(Builders<Exam>.Filter.Empty).ToListAsync();
    }


    public async Task<Exam> GetExmById(string id)
    {
        var filter = Builders<Exam>.Filter.Eq(s => s.Id, id);
        return await Collection.Find(filter).FirstOrDefaultAsync();
    }
}
