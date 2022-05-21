namespace Adasit.Bootstrap.Application.Interfaces;
using Adasit.Bootstrap.Application.Dto.Models.Events;

public interface IMessageSenderInterface
{
    Task Send(TopicNames topicName, object data);
}
