namespace Adasit.Bootstrap.ComponentTest.Utils;

using Adasit.Bootstrap.Application.Dto.Models.Events;
using Adasit.Bootstrap.Application.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

public class TopicMessageTestHelper : IMessageSenderInterface
{
    public List<EventList> list;

    public TopicMessageTestHelper()
    {
        list = new List<EventList>();
    }

    public Task Send(TopicNames name, object data)
    {
        list.Add(new EventList(name.Value, (DefaultMessageNotification)data));

        return Task.CompletedTask;
    }
}

public class EventList
{
    public EventList(string name, DefaultMessageNotification data)
    {
        Name = name;
        Data = data;
    }

    public string Name { get; set; }

    public DefaultMessageNotification Data { get; set; }
}
