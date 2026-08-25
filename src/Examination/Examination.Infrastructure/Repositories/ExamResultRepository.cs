using Examination.Infrastructure.SeedWork;
using Exemination.Domain.AggregateModels.ExamResultAggregate;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Examination.Infrastructure.Repositories;

public class ExamResultRepository : BaseRepository<ExamResult>, IExamResultRepository
{
    public ExamResultRepository(IMongoClient mongoClient, IOptions<ExamSettings> settings, string collection) : base(mongoClient, settings, collection)
    {
    }


    async Task<ExamResult> IExamResultRepository.GetDetails(string userId, string examId)
    {
        var filter = Builders<ExamResult>.Filter.Where(s => s.Id == id);
    return await Collection.Find(filter).FirstOrDefaultAsync();
    }

}
