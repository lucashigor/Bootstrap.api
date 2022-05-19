namespace Adasit.Bootstrap.Domain.SeedWork.ShearchableRepository;

public interface IShearchableRepository<TAggregate> where TAggregate : AgregateRoot
{
    Task<SearchOutput<TAggregate>> Search(SearchInput input, CancellationToken cancellationToken);
}
