namespace Adasit.Bootstrap.Application.Notifications;

using Adasit.Bootstrap.Application.Interfaces;
using Adasit.Bootstrap.Application.Models;
using Hangfire;
using MediatR;
using System.Threading.Tasks;
public class DefaultNotification : INotificationHandler<PublishNotificationsEvents>
{
    public readonly IMessageSenderInterface message;

    public DefaultNotification(IMessageSenderInterface message)
    {
        this.message = message;
    }

    public Task Handle(PublishNotificationsEvents notification, CancellationToken cancellationToken)
    {
        BackgroundJob.Enqueue(() => message.Send(notification.TopicName, notification.Data));

        return Task.CompletedTask;
    }
}
