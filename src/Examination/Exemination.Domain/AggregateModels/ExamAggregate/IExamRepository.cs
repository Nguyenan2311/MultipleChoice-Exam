using Exemination.Domain.AggregateModels.SeedWork;

namespace Exemination.Domain.AggregateModels.ExamAggregate;

public interface IExamRepository: IRepositoryBase<Exam>
{
     Task<IEnumerable<Exam>> GetExamListAsync();
     Task<Exam> GetExmById(string id);

}
