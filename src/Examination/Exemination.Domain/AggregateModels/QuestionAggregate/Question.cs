using Examination.Dtos.Enums;
using MongoDB.Bson.Serialization.Attributes;
using Exemination.Domain.AggregateModels.SeedWork;

namespace Exemination.Domain.AggregateModels.QuestionAggregate;

public class Question : Entity,IAggregateRoot
{
    private Question()
    {
        Content = string.Empty;
        CategoryId = string.Empty;
        Answers = [];
        Explain = string.Empty;
        OwnerUserId = string.Empty;
    }

    public Question(
        string content,
        QuestionType questionType,
        Level level,
        string categoryId,
        IEnumerable<Answer> answers,
        string explain,
        string ownerUserId)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Content can not be empty.", nameof(content));
        }

        if (string.IsNullOrWhiteSpace(categoryId))
        {
            throw new ArgumentException("CategoryId can not be empty.", nameof(categoryId));
        }

        if (string.IsNullOrWhiteSpace(ownerUserId))
        {
            throw new ArgumentException("OwnerUserId can not be empty.", nameof(ownerUserId));
        }

        ArgumentNullException.ThrowIfNull(answers);

        var answerList = answers.ToList();
        if (answerList.Count == 0)
        {
            throw new ArgumentException("At least one answer is required.", nameof(answers));
        }

        Content = content;
        QuestionType = questionType;
        Level = level;
        CategoryId = categoryId;
        Answers = answerList;
        Explain = explain;
        DateCreated = DateTime.UtcNow;
        OwnerUserId = ownerUserId;
    }

    [BsonElement("content")]
    public string Content { get; set; }

    [BsonElement("questionType")]
    public QuestionType QuestionType { get; set; }

    [BsonElement("level")]
    public Level Level { get; set; }

    [BsonElement("categoryId")]
    public string CategoryId { get; set; }

    [BsonElement("answers")]
    public IEnumerable<Answer> Answers { get; set; }

    [BsonElement("explain")]
    public string Explain { get; set; }

    [BsonElement("dateCreated")]
    public DateTime DateCreated { get; set; }

    [BsonElement("ownerUserId")]
    public string OwnerUserId { get; set; }
}