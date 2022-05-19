namespace Adasit.Bootstrap.Application.Dto.Models.Response;
public class ListConfigurationsOutput
    : PaginatedListOutput<ConfigurationOutputDto>
{
    public ListConfigurationsOutput(
        int page,
        int perPage,
        int total,
        IReadOnlyList<ConfigurationOutputDto> items)
        : base(page, perPage, total, items)
    {
    }
}
