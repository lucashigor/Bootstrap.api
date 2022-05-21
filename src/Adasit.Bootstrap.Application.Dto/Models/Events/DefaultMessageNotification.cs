using Adasit.Bootstrap.Application.Dto.Models.Errors;

namespace Adasit.Bootstrap.Application.Dto.Models.Events;

public class DefaultMessageNotification
{
    public DefaultMessageNotification(EventNames eventName, object data)
    {
        EventName = eventName;
        Data = data;

        Validade();
    }

    public EventNames EventName { get; private set; }
    public object Data { get; private set; }

    private void Validade()
    {
        if (Data is null || EventName is null)
        {
            throw new BusinessException(ErrorCodeConstant.NotificationValuesError());
        }
    }
}
