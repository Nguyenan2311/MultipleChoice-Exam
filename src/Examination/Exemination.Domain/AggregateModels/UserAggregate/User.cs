using Exemination.Domain.AggregateModels.SeedWork;
using MongoDB.Bson.Serialization.Attributes;

namespace Exemination.Domain.AggregateModels.UserAggregate;

public class User:Entity,IAggregateRoot

{
    public User(string externalId, string firstName, string lastName)
    {
        this.ExternalId = externalId;
        this.FirstName = firstName;
        this.LastName = lastName;
    }

    [BsonElement("externalId")]
    public string ExternalId { get; set; }
    [BsonElement("firstName")]
    public string FirstName { get; set; }
    [BsonElement("lastName")]
    public string LastName { get; set; }

    public static User CreateNewUser(string externalId, string firstName, string lastName)
    {
        return new User(externalId, firstName, lastName);
    }

}
