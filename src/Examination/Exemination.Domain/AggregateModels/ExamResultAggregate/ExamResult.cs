using Exemination.Domain.AggregateModels.ExamAggregate;
using MongoDB.Bson.Serialization.Attributes;
using Exemination.Domain.AggregateModels.SeedWork;
using Exemination.Domain.Events;

namespace Exemination.Domain.AggregateModels.ExamResultAggregate;

public class ExamResult : Entity,IAggregateRoot
{
	

	public ExamResult(string userId, string examId)
	{
		UserId = userId;
		ExamId = examId;
		ExamResultDetails = [];
		ExamStartDate = DateTime.UtcNow;
		ExamFinishDate = null;
		Passed = false;
		Finished = false;
	}

	[BsonElement("examId")]
	public string ExamId { get; set; }

	[BsonElement("userId")]
	public string UserId { get; set; }

	[BsonElement("examQuestionReviews")]
	public IEnumerable<ExamResultDetail> ExamResultDetails { get; set; }

	[BsonElement("examDate")]
	public DateTime ExamStartDate { get; set; }

	[BsonElement("examFinishDate")]
	public DateTime? ExamFinishDate { get; set; }

	[BsonElement("passed")]
	public bool Passed { get; set; }

	[BsonElement("finished")]
	public bool Finished { get; set; }

	public static ExamResult CreateNewResult(string userId, string examId)
	{
		var result = new ExamResult(userId, examId);
        return result;
	}

	public void StartExam(string firstName, string lastName)
	{
		AddDomainEvent(new ExamStartDomainEvent(UserId, firstName, lastName));
	}

	public void SetUserChoices(List<ExamResultDetail> examResultDetails)
	{
		ExamResultDetails = examResultDetails;
	}

	public void Finish()
	{
		Finished = true;
		ExamFinishDate = DateTime.Now;
	}
}
