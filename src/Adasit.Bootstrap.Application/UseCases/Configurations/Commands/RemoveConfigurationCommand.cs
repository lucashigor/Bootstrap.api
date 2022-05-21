namespace Adasit.Bootstrap.Application.UseCases.Configurations.Commands;
using Adasit.Bootstrap.Application.Dto.Models.Errors;
using Adasit.Bootstrap.Application.Interfaces;
using Adasit.Bootstrap.Application.Models;
using Adasit.Bootstrap.Domain.Exceptions;
using Adasit.Bootstrap.Domain.Repository;
using MediatR;

public class RemoveConfigurationInput : IRequest
{
    public Guid Id { get; private set; }
    public RemoveConfigurationInput(Guid Id)
    {
        this.Id = Id;
    }
}

public class RemoveConfigurationCommand : BaseCommands, IRequestHandler<RemoveConfigurationInput>
{
    public IConfigurationRepository repository;
    public IUnitOfWork unitOfWork;
    public IDateValidationHandler dateValidationHandler;

    public RemoveConfigurationCommand(IConfigurationRepository repository,
        IUnitOfWork unitOfWork,
        Notifier notifier,
        IDateValidationHandler dateValidationHandler) : base(notifier)
    {
        this.repository = repository;
        this.unitOfWork = unitOfWork;
        this.dateValidationHandler = dateValidationHandler;
    }

    public async Task<Unit> Handle(RemoveConfigurationInput request, CancellationToken cancellationToken)
    {
        try
        {
            var item = await repository.GetById(request.Id, cancellationToken);

            if (item is null)
            {
                this.notifier.Warnings.Add(ErrorCodeConstant.ConfigurationNotFound());
                return Unit.Value;
            }

            if(item.StartDate > DateTimeOffset.UtcNow && item.FinalDate > DateTimeOffset.UtcNow)
            {
                var err = ErrorCodeConstant.ThisCannotBeDoneOnClosedConfiguration();

                this.notifier.Erros.Add(err);

                return Unit.Value;
            }

            if(item.StartDate < DateTimeOffset.UtcNow
                && item.FinalDate > DateTimeOffset.UtcNow)
            {
                item.SetFinalDateToNow();

                var err = ErrorCodeConstant.ConfigurationInCourse();

                err.ChangeInnerMessage("The final date was set to now");

                this.notifier.Warnings.Add(err);

                await repository.Update(item, cancellationToken);
            }
            else
            {
                await repository.Delete(item, cancellationToken);
            }

            await unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
        catch (EntityGenericException ex)
        {
            HanddlerEntityGenericException(ex);

            return Unit.Value;
        }
        catch (Exception ex)
        {
            HanddlerGenericException(ex);
            return Unit.Value;
        }
    }
}