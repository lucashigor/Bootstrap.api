namespace Adasit.Bootstrap.Application.UseCases.Configurations.Queries;
using Adasit.Bootstrap.Application.Dto.Models.Errors;
using Adasit.Bootstrap.Application.Dto.Models.Response;
using Adasit.Bootstrap.Application.Models;
using Adasit.Bootstrap.Application.UseCases;
using Adasit.Bootstrap.Application.UseCases.Configurations.Mappers;
using Adasit.Bootstrap.Domain.Repository;
using MediatR;

public class GetConfigurationInput : IRequest<ConfigurationOutputDto>
{
    public Guid Id { get; private set; }
    public GetConfigurationInput(Guid Id)
    {
        this.Id = Id;
    }
}

public class GetByIdConfigurationQuery : BaseCommands, IRequestHandler<GetConfigurationInput, ConfigurationOutputDto>
{
    public IConfigurationRepository repository;

    public GetByIdConfigurationQuery(IConfigurationRepository repository,
        Notifier notifier) : base(notifier)
    {
        this.repository = repository;
    }

    public async Task<ConfigurationOutputDto> Handle(GetConfigurationInput request, CancellationToken cancellationToken)
    {
        var item = await repository.GetById(request.Id, cancellationToken);

        if (item is null)
        {
            notifier.Warnings.Add(ErrorCodeConstant.ConfigurationNotFound());

            return null!;
        }

        return item.MapDtoFromDomain();
    }
}