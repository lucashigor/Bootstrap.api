namespace Adasit.Bootstrap.Application.UseCases.Configurations.Mappers;

using Adasit.Bootstrap.Application.Dto.Models.Request;
using Adasit.Bootstrap.Application.Dto.Models.Response;
using Adasit.Bootstrap.Application.UseCases.Configurations.Commands;
using Adasit.Bootstrap.Domain.Entity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

public static class ConfigurationMapper
{
    public static ConfigurationOutputDto MapDtoFromDomain(this Configuration item)
        => new (item.Id, item.Name, item.Value, item.Description, item.StartDate, item.FinalDate);

	public static JsonPatchDocument<Configuration> MapInputToDomain(this JsonPatchDocument<ModifyConfigurationInput> source)
	{
		if (source is null)
		{
			return null!;
		}

		return new (
			source.Operations.Select(operation => 
				new Operation<Configuration>(operation.op, operation.path, operation.from, operation.value)).ToList(),
			source.ContractResolver);
	}

	public static JsonPatchDocument<ModifyConfigurationInput> MapDtoToInput(this JsonPatchDocument<ModifyConfigurationInputDto> source)
	{
		if (source is null)
		{
			return null!;
		}

		return new(
			source.Operations.Select(operation =>
				new Operation<ModifyConfigurationInput>(operation.op, operation.path, operation.from, operation.value)).ToList(),
			source.ContractResolver);
	}
}
