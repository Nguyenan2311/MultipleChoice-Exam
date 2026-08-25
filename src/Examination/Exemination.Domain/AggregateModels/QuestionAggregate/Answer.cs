using MongoDB.Bson.Serialization.Attributes;
using Exemination.Domain.AggregateModels.SeedWork;

namespace Exemination.Domain.AggregateModels.QuestionAggregate;

public class Answer : Entity
{
    private Answer()
    {
        Content = string.Empty;
    }

    public Answer(string content, bool isCorrect)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Content can not be empty.", nameof(content));
        }

        Content = content;
        IsCorrect = isCorrect;
    }

    [BsonElement("content")]
    public string Content { get; set; } = string.Empty;

    [BsonElement("isCorrect")]
    public bool IsCorrect { get; set; }
}