namespace Adasit.Bootstrap.Application.Dto.Models;

public class ErrorModel
{
    public string Code { get; private set; }
    public string Message { get; private set; }
    public string InnerMessage { get; private set; }

    public ErrorModel(string code, string message)
    {
        Code = code;
        Message = message;
        InnerMessage = string.Empty;
    }

    public ErrorModel(string code, string message, string innerMessage)
    {
        Code = code;
        Message = message;
        InnerMessage = innerMessage;
    }

    public void ChangeInnerMessage(string message)
    {
        InnerMessage = message;
    }
}
