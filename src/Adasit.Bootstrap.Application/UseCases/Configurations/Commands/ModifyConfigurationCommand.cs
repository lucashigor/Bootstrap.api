namespace Adasit.Bootstrap.Application.UseCases.Configurations.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;
using Adasit.Bootstrap.Application.Dto;
using Adasit.Bootstrap.Application.Dto.Models.Errors;
using Adasit.Bootstrap.Application.Dto.Models.Response;
using Adasit.Bootstrap.Application.Models;
using Adasit.Bootstrap.Domain.Entity;
using Adasit.Bootstrap.Domain.Repository;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

public class PatchConfiguration : IRequest<ConfigurationOutputDto>
{
    public JsonPatchDocument<ModifyConfigurationInput> PatchDocument { get; set; }
    public Guid Id { get; set; }

    public PatchConfiguration(Guid Id, JsonPatchDocument<ModifyConfigurationInput> PatchDocument)
    {
        this.PatchDocument = PatchDocument;
        this.Id = Id;
    }
}

public class ModifyConfigurationCommand : IRequestHandler<PatchConfiguration, ConfigurationOutputDto>
{
    private readonly IConfigurationRepository repository;
    private readonly Notifier notifier;
    private readonly IMediator mediator;

    public ModifyConfigurationCommand(IConfigurationRepository repository,
        Notifier notifier,
        IMediator mediator)
    {
        this.repository = repository;
        this.notifier = notifier;
        this.mediator = mediator;
    }

    public async Task<ConfigurationOutputDto> Handle(PatchConfiguration command, CancellationToken cancellationToken)
    {
        try
        {
            command.PatchDocument.Validate(
                   OperationType.Replace,
                   new List<string> { $"/{nameof(Configuration.Name)}",
                    $"/{nameof(Configuration.Value)}",
                    $"/{nameof(Configuration.Description)}",
                    $"/{nameof(Configuration.StartDate)}",
                    $"/{nameof(Configuration.FinalDate)}" }
                   );
        }
        catch (BusinessException ex)
        {
            notifier.Erros.Add(ex.ErrorCode);
            return null!;
        }

        var entity = await repository.GetById(command.Id, cancellationToken);

        if (entity == null)
        {
            notifier.Erros.Add(ErrorCodeConstant.ConfigurationNotFound());
            return null!;
        }

        var oldItem = new ModifyConfigurationInput(
            entity.Id,
            entity.Name,
            entity.Value,
            entity.Description,
            entity.StartDate,
            entity.FinalDate);

        command.PatchDocument.ApplyTo(oldItem);

        var ret = await mediator.Send(oldItem, cancellationToken);

        return ret;
    }
}
