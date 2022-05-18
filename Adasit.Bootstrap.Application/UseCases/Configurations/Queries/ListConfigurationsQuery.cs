namespace Adasit.Bootstrap.Application.UseCases.Configurations.Queries;

using Adasit.Bootstrap.Application.Dto.Models.Request;
using Adasit.Bootstrap.Application.Dto.Models.Response;
using Adasit.Bootstrap.Application.UseCases.Configurations.Mappers;
using Adasit.Bootstrap.Domain.Repository;
using Adasit.Bootstrap.Domain.SeedWork.ShearchableRepository;
using MediatR;

public class ListConfigurationsInput
    : PaginatedListInput, IRequest<ListConfigurationsOutput>
{
    public ListConfigurationsInput(
        int page = 1,
        int perPage = 15,
        string search = "",
        string sort = "",
        Dto.Models.SearchOrder dir = Dto.Models.SearchOrder.Asc
    ) : base(page, perPage, search, sort, dir)
    { }

    public ListConfigurationsInput()
        : base(1, 15, "", "", Dto.Models.SearchOrder.Asc)
    { }
}

public class ListConfigurationsQuery : IListConfigurations
{
    private readonly IConfigurationRepository configurationRepository;

    public ListConfigurationsQuery(IConfigurationRepository categoryRepository)
        => configurationRepository = categoryRepository;

    public async Task<ListConfigurationsOutput> Handle(
        ListConfigurationsInput request,
        CancellationToken cancellationToken)
    {
        var searchOutput = await configurationRepository.Search(
            new(
                request.Page,
                request.PerPage,
                request.Search,
                request.Sort,
                (SearchOrder)request.Dir
            ),
            cancellationToken
        );

        return new ListConfigurationsOutput(
            searchOutput.CurrentPage,
            searchOutput.PerPage,
            searchOutput.Total,
            searchOutput.Items
                .Select(ConfigurationMapper.MapDtoFromDomain)
                .ToList()
        );
    }
}