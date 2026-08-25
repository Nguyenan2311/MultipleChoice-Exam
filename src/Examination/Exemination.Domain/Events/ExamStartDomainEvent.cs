using MediatR;

namespace Exemination.Domain.Events;

public class ExamStartDomainEvent : INotification
{
    public ExamStartDomainEvent(string userId, string firstName, string lastName)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
    }

    public string UserId { get; }
    public string FirstName { get; }
    public string LastName { get; }
}