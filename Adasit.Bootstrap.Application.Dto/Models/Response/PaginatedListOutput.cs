namespace Adasit.Bootstrap.Application.Dto.Models.Response;

public class PaginatedListOutput<T>
{
    public int Page { get; set; }
    public int PerPage { get; set; }
    public int Total { get; set; }
    public IReadOnlyList<T> Items { get; set; }

    protected PaginatedListOutput(
        int page,
        int perPage,
        int total,
        IReadOnlyList<T> items)
    {
        Page = page;
        PerPage = perPage;
        Total = total;
        Items = items;
    }
}
