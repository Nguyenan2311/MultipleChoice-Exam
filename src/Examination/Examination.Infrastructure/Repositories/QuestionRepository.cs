using Examination.Infrastructure.SeedWork;
using Exemination.Domain.AggregateModels.QuestionAggregate;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Examination.Infrastructure.Repositories;

public class QuestionRepository : BaseRepository<Question>, IQuestionRepository
{
    public QuestionRepository(IMongoClient mongoClient, IOptions<ExamSettings> settings, string collection) : base(mongoClient, settings, collection)
    {
    }

}
