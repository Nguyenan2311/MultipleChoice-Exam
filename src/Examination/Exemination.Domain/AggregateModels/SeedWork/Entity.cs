using MediatR;
using MongoDB.Bson.Serialization.Attributes;

namespace Exemination.Domain.AggregateModels.SeedWork;

public abstract class Entity
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    [BsonElement("_id")]
    public virtual string Id {get; protected set; }
    private List<INotification> _domainEvents;

    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

    public void AddDomainEvent(INotification eventItem)
    {
        _domainEvents ??= new List<INotification>();
        _domainEvents.Add(eventItem);
    }
    public void RemoveDomainEvent(INotification eventItem)
    {
        _domainEvents?.Remove(eventItem);
    }
    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }
    public bool IsTransient()
    {
        return Id == default;

    }
    public override bool Equals(object obj)
    {
        if (obj == null || !(obj is Entity))
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (GetType() != obj.GetType())
            return false;

        var item = (Entity)obj;

        if (item.IsTransient() || IsTransient())
            return false;
        else
            return item.Id == Id;
    }
    public static bool operator !=(Entity left, Entity right)
    {
        return !(left == right);
    }

    public static bool operator ==(Entity left, Entity right)
    {
        if (ReferenceEquals(left, right))
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }
    public override int GetHashCode()
    {
        if (IsTransient())
            return base.GetHashCode();
        else
            return Id.GetHashCode() ^ 31;
    }


}
