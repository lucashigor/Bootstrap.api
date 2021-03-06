namespace Adasit.Bootstrap.Application.UseCases;

using Adasit.Bootstrap.Application.Dto.Models.Errors;
using Adasit.Bootstrap.Application.Models;
using Adasit.Bootstrap.Domain.Conts;
using Adasit.Bootstrap.Domain.Exceptions;

public abstract class BaseCommands
{
    public Notifier notifier { get; private set; }
    public BaseCommands(Notifier notifier)
    {
        this.notifier = notifier;
    }

    protected void HanddlerEntityGenericException(EntityGenericException ex)
    {
        var errors = ex.Message.Split(";");

        foreach (var item in errors)
        {
            ErrorModel message;

            var err = item.Split(":");

            message = int.Parse(err[0]) switch
            {
                (int)ErrorsCodes.Validation => ErrorCodeConstant.Validation(),
                (int)ErrorsCodes.ConfigurationDateConflit => ErrorCodeConstant.StartDateCannotBeBeforeToUtcNow(),
                _ => ErrorCodeConstant.ErrorOnSavingNewConfiguration()
            };

            message.ChangeInnerMessage(err[1]);

            notifier.Erros.Add(message);
        }
    }

    protected void HanddlerGenericException(Exception ex)
    {
        var message = ErrorCodeConstant.ErrorOnSavingNewConfiguration();

        message.ChangeInnerMessage(ex.Message);

        notifier.Erros.Add(message);
    }
}
