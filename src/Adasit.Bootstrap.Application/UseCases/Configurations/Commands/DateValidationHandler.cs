namespace Adasit.Bootstrap.Application.UseCases.Configurations.Commands;
using System.Linq;
using System.Threading.Tasks;
using Adasit.Bootstrap.Application.Dto.Models.Errors;
using Adasit.Bootstrap.Application.Models;
using Adasit.Bootstrap.Domain.Entity;
using Adasit.Bootstrap.Domain.Repository;

public class DateValidationHandler : IDateValidationHandler
{
    private readonly IConfigurationRepository configurationRepository;
    private readonly Notifier notifier;

    public DateValidationHandler(IConfigurationRepository configurationRepository,
        Notifier notifier)
    {
        this.configurationRepository = configurationRepository;
        this.notifier = notifier;
    }

    public async Task Handle(Configuration entity, CancellationToken cancellationToken)
    {
        var listWithSameName = await configurationRepository.GetByName(entity.Name, cancellationToken);

        if (listWithSameName is not null && listWithSameName.Where(x => x.Id != entity.Id).Any())
        {
            if (listWithSameName.Where(x => x.StartDate < entity.StartDate && x.FinalDate > entity.StartDate && x.Id != entity.Id).Any())
            {
                notifier.Erros.Add(ErrorCodeConstant.ThereWillCurrentConfigurationStartDate());
            }

            if (listWithSameName.Where(x => x.StartDate < entity.FinalDate && x.FinalDate > entity.FinalDate && x.Id != entity.Id).Any())
            {
                notifier.Erros.Add(ErrorCodeConstant.ThereWillCurrentConfigurationEndDate());
            }
        }
    }
}
