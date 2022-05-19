namespace Adasit.Bootstrap.Application.UseCases.Configurations.Queries;

using Adasit.Bootstrap.Application.Dto.Models.Response;
using MediatR;

public interface IListConfigurations : IRequestHandler<ListConfigurationsInput, ListConfigurationsOutput>
{ }