namespace Adasit.Bootstrap.Application.UseCases.Configurations.Commands;
using System.Threading;
using System.Threading.Tasks;
using Adasit.Bootstrap.Application.Dto.Models;
using Adasit.Bootstrap.Application.Dto.Models.Errors;
using Adasit.Bootstrap.Application.Dto.Models.Events;
using Adasit.Bootstrap.Application.Dto.Models.Response;
using Adasit.Bootstrap.Application.Interfaces;
using Adasit.Bootstrap.Application.Models;
using Adasit.Bootstrap.Application.UseCases.Configurations.Mappers;
using Adasit.Bootstrap.Domain.Conts;
using Adasit.Bootstrap.Domain.Exceptions;
using Adasit.Bootstrap.Domain.Repository;
using MediatR;

public class ChangeConfigurationCommand : BaseCommands, IRequestHandler<ModifyConfigurationInput, ConfigurationOutputDto>
{
    private readonly IConfigurationRepository repository;
    private readonly IUnitOfWork unitOfWork;
    private readonly IDateValidationHandler dateValidationHandler;
    private readonly IMediator mediator;

    public ChangeConfigurationCommand(IConfigurationRepository repository,
        IUnitOfWork unitOfWork,
        IDateValidationHandler dateValidationHandler,
        IMediator mediator,
        Notifier notifier) : base(notifier)
    {
        this.repository = repository;
        this.unitOfWork = unitOfWork;
        this.dateValidationHandler = dateValidationHandler;
        this.mediator = mediator;
    }

    public async Task<ConfigurationOutputDto> Handle(ModifyConfigurationInput command, CancellationToken cancellationToken)
    {
        try
        {
            ConfigurationOutputDto ret = null!;

            var entity = await repository.GetById(command.Id, cancellationToken);

            if (entity == null)
            {
                notifier.Erros.Add(ErrorCodeConstant.ConfigurationNotFound());
                return null!;
            }

            if (entity.StartDate < DateTimeOffset.UtcNow 
                && entity.FinalDate < DateTimeOffset.UtcNow 
                && (entity.StartDate != command.StartDate
                    || entity.FinalDate != command.FinalDate
                    || entity.Name != command.Name
                    || entity.Value != command.Value))
            {
                notifier.Erros.Add(ErrorCodeConstant.OnlyDescriptionAreAvaliableToChangedOnClosedConfiguration());
                return null!;
            }

            if (entity.StartDate < DateTimeOffset.UtcNow 
                && entity.FinalDate > DateTimeOffset.UtcNow
                && entity.Name != command.Name)
            {
                notifier.Erros.Add(ErrorCodeConstant.ItsNotAllowedToChangeName());
                return null!;
            }

            if (entity.StartDate < DateTimeOffset.UtcNow 
                && entity.FinalDate > DateTimeOffset.UtcNow
                && entity.StartDate != command.StartDate)
            {
                notifier.Erros.Add(ErrorCodeConstant.ItsNotAllowedToChangeInitialDate());
                return null!;
            }

            if (entity.StartDate < DateTimeOffset.UtcNow
                && entity.FinalDate > DateTimeOffset.UtcNow
                && entity.FinalDate != command.FinalDate
                && command.FinalDate < DateTimeOffset.UtcNow)
            {
                notifier.Warnings.Add(ErrorCodeConstant.ItsNotAllowedToChangeFinalDatetoBeforeToday());
                command.FinalDate = DateTimeOffset.UtcNow;
            }

            if (entity.StartDate < DateTimeOffset.UtcNow 
                && entity.FinalDate > DateTimeOffset.UtcNow 
                && entity.Value != command.Value)
            {
                entity.Modify(entity.Name,
                entity.Value,
                entity.Description,
                entity.StartDate,
                DateTimeOffset.UtcNow);

                await repository.Update(entity, cancellationToken);

                var newOne = new RegisterConfigurationInput()
                {
                    Name = command.Name,
                    Value = command.Value,
                    Description = command.Description,
                    StartDate = DateTimeOffset.UtcNow,
                    FinalDate = command.FinalDate
                };

                ret = await mediator.Send(newOne, cancellationToken);
            }
            else
            {
                entity.Modify(command.Name,
                    command.Value,
                    command.Description,
                    command.StartDate,
                    command.FinalDate);

                await dateValidationHandler.Handle(entity, cancellationToken);

                await repository.Update(entity, cancellationToken);

                ret = entity.MapDtoFromDomain();
            }

            var message = new DefaultMessageNotification(EventNames.ConfigurationChanged, entity);

            await mediator.Publish(new PublishNotificationsEvents(TopicNames.Bootstrap_Topic, message), cancellationToken);

            await unitOfWork.CommitAsync(cancellationToken);

            return ret;

        }
        catch (EntityGenericException ex)
        {
            HanddlerEntityGenericException(ex);

            return null!;
        }
        catch (Exception ex)
        {
            HanddlerGenericException(ex);
            return null!;
        }
    }
}
