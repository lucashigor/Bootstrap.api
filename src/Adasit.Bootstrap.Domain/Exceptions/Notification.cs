namespace Adasit.Bootstrap.Domain.Exceptions;
using Adasit.Bootstrap.Domain.Conts;

public class Notification
{
    public string FieldName { get; private set; }
    public string Message { get; private set; }
    public ErrorsCodes ErrorCode { get; private set; }

    public Notification(string fieldName, string message, ErrorsCodes errorCode)
    {
        this.FieldName = fieldName;
        this.Message = message;
        this.ErrorCode = errorCode;
    }

    public static string GetMessage(List<Notification> list)
    {
        var ret = "";

        foreach (Notification notification in list)
        {
            if(ret.Length > 0)
            {
                ret += ";";
            }

            ret += $"{(int)notification.ErrorCode}:{notification.Message}";
        }
        return ret;
    }
}
