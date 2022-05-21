namespace Adasit.Bootstrap.Application.Dto.Models.Errors;

public class ErrorModel
{
    public ErrorCodes Code { get; private set; }
    public string Message { get; private set; }
    public string InnerMessage { get; private set; }

    public ErrorModel(ErrorCodes code, string message)
    {
        Code = code;
        Message = message;
        InnerMessage = string.Empty;
    }

    public ErrorModel(ErrorCodes code, string message, string innerMessage)
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
