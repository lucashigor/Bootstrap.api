namespace Adasit.Bootstrap.Application.UseCases.Configurations.Commands;

using System.Threading.Tasks;
using Adasit.Bootstrap.Domain.Entity;

public interface IDateValidationHandler
{
    Task Handle(Configuration entity, CancellationToken cancellationToken);
}
