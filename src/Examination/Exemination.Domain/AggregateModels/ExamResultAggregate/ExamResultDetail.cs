using Exemination.Domain.AggregateModels.QuestionAggregate;
using Exemination.Domain.AggregateModels.SeedWork;
using MongoDB.Bson.Serialization.Attributes;

namespace Exemination.Domain.AggregateModels.ExamResultAggregate;

public class ExamResultDetail : Entity
{
	private ExamResultDetail()
	{
		Question = null!;
		SelectedAnswers = [];
		Explain = string.Empty;
	}

	public ExamResultDetail(
		Question question,
		IEnumerable<Answer> selectedAnswers,
		string explain)
	{
		ArgumentNullException.ThrowIfNull(question);
		ArgumentNullException.ThrowIfNull(selectedAnswers);

		var selectedAnswerList = selectedAnswers.ToList();
		if (selectedAnswerList.Count == 0)
		{
			throw new ArgumentException("At least one selected answer is required.", nameof(selectedAnswers));
		}

		Question = question;
		SelectedAnswers = selectedAnswerList;
		Explain = explain;

		var correctAnswerIds = question.Answers
			.Where(answer => answer.IsCorrect)
			.Select(answer => answer.Id)
			.ToHashSet();
		var selectedAnswerIds = selectedAnswerList
			.Select(answer => answer.Id)
			.ToHashSet();

		IsCorrect = correctAnswerIds.SetEquals(selectedAnswerIds);
	}

	[BsonElement("question")]
	public Question Question { get; set; }

	[BsonElement("selectedAnswers")]
	public IEnumerable<Answer> SelectedAnswers { get; set; }

	[BsonElement("explain")]
	public string Explain { get; set; }

	[BsonElement("isCorrect")]
	public bool IsCorrect { get; set; }
}