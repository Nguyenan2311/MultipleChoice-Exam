using Exemination.Domain.AggregateModels.SeedWork;

namespace Exemination.Domain.AggregateModels.ExamResultAggregate;

public interface IExamResultRepository:IRepositoryBase<ExamResult>
{
    Task<ExamResult> GetDetails(string userId, string examId);

}
