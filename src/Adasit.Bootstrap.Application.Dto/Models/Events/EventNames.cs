namespace Adasit.Bootstrap.Application.Dto.Models.Events;

public class EventNames
{
    private EventNames(string value)
    {
        Value = value;
    }

    public string Value { get; private set; }

    public static EventNames ConfigurationCreated { get { return new EventNames("ConfigurationCreated"); } }
    public static EventNames ConfigurationChanged { get { return new EventNames("ConfigurationChanged"); } }
}
