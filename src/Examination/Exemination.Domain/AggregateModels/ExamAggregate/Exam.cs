using Examination.Dtos.Enums;
using Exemination.Domain.AggregateModels.SeedWork;
using Exemination.Domain.AggregateModels.QuestionAggregate;
using MongoDB.Bson.Serialization.Attributes;

namespace Exemination.Domain.AggregateModels.ExamAggregate;

public class Exam : Entity, IAggregateRoot
{
    private Exam()
        : this(string.Empty, string.Empty, string.Empty, 0, TimeSpan.Zero, [], default, string.Empty, 0, false)
    {
    }

    public Exam(
        string name,
        string shortDesc,
        string content,
        int numberOfQuestions,
        TimeSpan duration,
        IEnumerable<Question> questions,
        Level level,
        string ownerId,
        int numberOfQuestionCorrectForPass,
        bool isTimeRestricted)
    {
        ArgumentNullException.ThrowIfNull(questions);

        var questionList = questions.ToList();
        if (questionList.Count != numberOfQuestions)
        {
            throw new ArgumentException($"{nameof(numberOfQuestions)} is invalid.", nameof(numberOfQuestions));
        }

        if (numberOfQuestionCorrectForPass > numberOfQuestions)
        {
            throw new ArgumentException($"{nameof(numberOfQuestionCorrectForPass)} is invalid.", nameof(numberOfQuestionCorrectForPass));
        }

        Name = name;
        ShortDesc = shortDesc;
        Content = content;
        NumberOfQuestions = numberOfQuestions;
        Duration = duration;
        Questions = questionList;
        Level = level;
        OwnerId = ownerId;
        NumberOfQuestionCorrectForPass = numberOfQuestionCorrectForPass;
        IsTimeRestricted = isTimeRestricted;
        CreatedAt = DateTime.UtcNow;
    }


    [BsonElement("name")] public string Name { get; set; }
    [BsonElement("shortDesc")] public string ShortDesc { get; set; }
    [BsonElement("content")] public string Content { get; set; }
    [BsonElement("numberOfQuestion")] public int NumberOfQuestions { get; set; }

    [BsonElement("duration")] public TimeSpan Duration { get; set; }

    public IEnumerable<Question> Questions { get; set; }
    [BsonElement("level")] public Level Level { get; set; }
    [BsonElement("createdAt")] public DateTime CreatedAt { get; set; }
    [BsonElement("ownerId")] public string OwnerId { get; set; }

    [BsonElement("numberOfQuestionCorrectForPass")]
    public int NumberOfQuestionCorrectForPass { get; set; }

    [BsonElement("isTimeRestricted")] public bool IsTimeRestricted { get; set; }
}
