namespace Adasit.Bootstrap.Infrastructure.Repositories;

using System.Collections.Generic;
using System.Linq;
using Adasit.Bootstrap.Domain.Repository;
using Adasit.Bootstrap.Domain.SeedWork.ShearchableRepository;
using Adasit.Bootstrap.Infrastructure.Repositories.Context;
using Microsoft.EntityFrameworkCore;
using DomainEntity = Domain.Entity;

public class ConfigurationRepository : IConfigurationRepository
{
    private readonly DbSet<DomainEntity.Configuration> dbSet;

    public ConfigurationRepository(PrincipalContext context)
    {
        dbSet = context.Configuration;
    }

    public async Task Insert(DomainEntity.Configuration configuration, CancellationToken cancellationToken)
        => await dbSet.AddAsync(configuration, cancellationToken);

    public Task<DomainEntity.Configuration?> GetById(Guid Id, CancellationToken cancellationToken)
        => dbSet.FirstOrDefaultAsync(x => x.Id == Id, cancellationToken);

    public Task Update(DomainEntity.Configuration configuration, CancellationToken cancellationToken)
    {
        dbSet.Attach(configuration);
        dbSet.Update(configuration);

        return Task.CompletedTask;
    }

    public async Task Delete(DomainEntity.Configuration configuration, CancellationToken cancellationToken)
    {
        var ids = new object[] 
        {
            configuration.Id
        };

        var item = await dbSet.FindAsync(ids, cancellationToken);

        if(item != null)
        {
            dbSet.Remove(item);
        }
    }

    public async Task<SearchOutput<DomainEntity.Configuration>> Search(SearchInput input, CancellationToken cancellationToken)
    {
        var toSkip = (input.Page - 1) * input.PerPage;
        var query = dbSet.AsNoTracking();

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
        var item = await dbSet.Where(x => x.Name.Equals(name)).ToListAsync(cancellationToken);

        return item!;
    }
}
