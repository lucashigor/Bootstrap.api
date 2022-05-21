namespace Adasit.Bootstrap.Application.Dto.Models.Events;
public class TopicNames
{
    private TopicNames(string value) { Value = value; }

    public string Value { get; private set; }

    public static TopicNames Bootstrap_Topic { get { return new TopicNames("Bootstrap.Topic"); } }
}
