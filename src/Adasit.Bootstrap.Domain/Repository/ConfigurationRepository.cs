namespace Adasit.Bootstrap.Domain.Repository;

using Adasit.Bootstrap.Domain.Entity;
using Adasit.Bootstrap.Domain.SeedWork;
using Adasit.Bootstrap.Domain.SeedWork.ShearchableRepository;

public interface IConfigurationRepository : IRepository, IShearchableRepository<Configuration>
{
    Task<Configuration?> GetById(Guid Id, CancellationToken cancellationToken);
    Task<List<Configuration>> GetByName(string name, CancellationToken cancellationToken);
    Task Insert(Configuration configuration, CancellationToken cancellationToken);
    Task Delete(Configuration configuration, CancellationToken cancellationToken);
    Task Update(Configuration configuration, CancellationToken cancellationToken);
}