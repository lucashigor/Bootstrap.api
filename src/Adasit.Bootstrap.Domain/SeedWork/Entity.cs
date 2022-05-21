namespace Adasit.Bootstrap.Domain.SeedWork;

using Adasit.Bootstrap.Domain.Exceptions;
using Adasit.Bootstrap.Domain.Validation;
public abstract class Entity
{
    public Guid Id { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? LastUpdateAt { get; protected set; }

    protected List<Notification> Notifications { get; private set; }

    protected Entity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        Notifications = new ();
    }

    protected virtual void Validate()
    {
        AddNotification(Id.NotNull());

        if (Notifications.Any())
        {
            throw new EntityGenericException(Notification.GetMessage(Notifications));
        }
    }

    protected void AddNotification(Notification? notification)
    {
        if (notification != null)
        {
            Notifications.Add(notification);
        }
    }
}