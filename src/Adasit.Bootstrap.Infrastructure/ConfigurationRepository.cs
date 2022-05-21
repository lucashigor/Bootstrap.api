namespace Adasit.Bootstrap.Infrastructure;

using System.Collections.Generic;
using System.Linq;
using Adasit.Bootstrap.Domain.Repository;
using Adasit.Bootstrap.Domain.SeedWork.ShearchableRepository;
using Adasit.Bootstrap.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using DomainEntity = Domain.Entity;
public class ConfigurationRepository : IConfigurationRepository
{
    private readonly DbSet<DomainEntity.Configuration> dbset;

    public ConfigurationRepository(PrincipalContext context)
    {
        dbset = context.Configuration;
    }

    public async Task Insert(DomainEntity.Configuration configuration, CancellationToken cancellationToken)
        => await dbset.AddAsync(configuration, cancellationToken);

    public async Task<DomainEntity.Configuration> GetById(Guid Id, CancellationToken cancellationToken)
        => await dbset.FirstOrDefaultAsync(x => x.Id == Id, cancellationToken);

    public Task Update(DomainEntity.Configuration user, CancellationToken cancellationToken)
    {
        dbset.Attach(user);

        return Task.CompletedTask;
    }

    public Task Delete(DomainEntity.Configuration user, CancellationToken cancellationToken)
    {
        dbset.Remove(user);

        return Task.CompletedTask;
    }

    public async Task<SearchOutput<DomainEntity.Configuration>> Search(SearchInput input, CancellationToken cancellationToken)
    {
        var toSkip = (input.Page - 1) * input.PerPage;
        var query = dbset.AsNoTracking();

        query = AddOrderToquery(query, input.OrderBy, input.Order);
        if (!string.IsNullOrWhiteSpace(input.Search))
            query = query.Where(x => x.Name.Contains(input.Search));

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip(toSkip)
            .Take(input.PerPage)
            .ToListAsync(cancellationToken);

        return new(input.Page, input.PerPage, total, items);
    }

    private static IQueryable<DomainEntity.Configuration> AddOrderToquery(
        IQueryable<DomainEntity.Configuration> query,
        string orderProperty,
        SearchOrder order)
    => (orderProperty.ToLower(), order) switch
    {
        ("name", SearchOrder.Asc) => query.OrderBy(x => x.Name),
        ("name", SearchOrder.Desc) => query.OrderByDescending(x => x.Name),
        ("value", SearchOrder.Asc) => query.OrderBy(x => x.Value),
        ("value", SearchOrder.Desc) => query.OrderByDescending(x => x.Value),
        _ => query.OrderBy(x => x.Name)
    };

    public async Task<List<DomainEntity.Configuration>> GetByName(string name, CancellationToken cancellationToken)
    {
        var item = await dbset.Where(x => x.Name.Equals(name)).ToListAsync(cancellationToken);

        return item!;
    }
}
