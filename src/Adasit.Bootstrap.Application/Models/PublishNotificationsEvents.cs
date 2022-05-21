namespace Adasit.Bootstrap.Application.Models;

using Adasit.Bootstrap.Application.Dto.Models.Events;
using MediatR;

public class PublishNotificationsEvents : INotification
{
    public TopicNames TopicName { get; private set; }
    public DefaultMessageNotification Data { get; private set; }

    public PublishNotificationsEvents(TopicNames TopicName, DefaultMessageNotification Data)
    {
        this.TopicName = TopicName;
        this.Data = Data;
    }
}
