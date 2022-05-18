namespace Adasit.Bootstrap.Application.UseCases.Configurations.Commands;

using Adasit.Bootstrap.Application.Dto;
using Adasit.Bootstrap.Application.Dto.Models.Request;
using Adasit.Bootstrap.Application.Dto.Models.Response;
using Adasit.Bootstrap.Application.Interfaces;
using Adasit.Bootstrap.Application.Models;
using Adasit.Bootstrap.Domain.Conts;
using Adasit.Bootstrap.Domain.Entity;
using Adasit.Bootstrap.Domain.Exceptions;
using Adasit.Bootstrap.Domain.Repository;
using MediatR;

public class RegisterConfigurationInput : IRequest<ConfigurationOutputDto>
{
    public string Name { get; set; }
    public string Value { get; set; }
    public string Description { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset FinalDate { get; set; }

    public RegisterConfigurationInput()
    {
        Name = string.Empty;
        Value = string.Empty;
        Description = string.Empty;
    }

    public RegisterConfigurationInput(RegisterConfigurationInputDto dto)
    {
        Name = dto.Name;
        Value = dto.Value;
        Description = dto.Description;
        StartDate = dto.StartDate;
        FinalDate = dto.FinalDate;
    }
}

public class RegisterConfigurationCommand : BaseCommands, IRequestHandler<RegisterConfigurationInput, ConfigurationOutputDto>
{
    public IConfigurationRepository repository;
    public IUnitOfWork unitOfWork;
    public Notifier notifier;
    public IDateValidationHandler dateValidationHandler;

    public RegisterConfigurationCommand(IConfigurationRepository repository,
        IUnitOfWork unitOfWork,
        Notifier notifier,
        IDateValidationHandler dateValidationHandler) : base(notifier)
    {
        this.repository = repository;
        this.unitOfWork = unitOfWork;
        this.notifier = notifier;
        this.dateValidationHandler = dateValidationHandler;
    }

    public async Task<ConfigurationOutputDto> Handle(RegisterConfigurationInput request, CancellationToken cancellationToken)
    {
        try
        {
            var item = new Configuration(request.Name,
                request.Value,
                request.Description,
                request.StartDate,
                request.FinalDate);

            if (item.FinalDate != (default) && item.FinalDate.AddHours(-1) < item.StartDate)
            {
                var err = ErrorCodeConstant.TheMinimunDurationIsOneHour();

                err.ChangeInnerMessage(ErrorsMessages.FinalDateCannotBeBeforeStartDate);

                notifier.Erros.Add(err);
            }

            if (item.FinalDate < DateTimeOffset.UtcNow)
            {
                notifier.Erros.Add(ErrorCodeConstant.EndDateCannotBeBeforeToToday());

                return null!;
            }

            if (item.StartDate < DateTimeOffset.UtcNow)
            {
                try
                {
                    item.FixStartDate();
                    notifier.Warnings.Add(ErrorCodeConstant.StartDateCannotBeBeforeToToday());
                }
                catch (EntityGenericException ex)
                {
                    if (ex.Code.Contains(ErrorsCodes.ConfigurationDateConflit))
                    {
                        notifier.Erros.Add(ErrorCodeConstant.StartDateCannotBeBeforeToToday());

                        return null!;
                    }

                    throw;
                }
            }


            await dateValidationHandler.Handle(item, cancellationToken);

            if (notifier.Erros.Any())
            {
                return null!;
            }

            await repository.Insert(item, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);

            return new ConfigurationOutputDto(item.Id, item.Name, item.Value, item.Description, item.StartDate, item.FinalDate);
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