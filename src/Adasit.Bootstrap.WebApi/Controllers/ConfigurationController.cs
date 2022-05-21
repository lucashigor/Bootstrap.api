namespace Adasit.Bootstrap.WebApi.Controllers;

using Adasit.Bootstrap.Application.Dto.Models;
using Adasit.Bootstrap.Application.Dto.Models.Request;
using Adasit.Bootstrap.Application.Dto.Models.Response;
using Adasit.Bootstrap.Application.Models;
using Adasit.Bootstrap.Application.UseCases.Configurations.Commands;
using Adasit.Bootstrap.Application.UseCases.Configurations.Mappers;
using Adasit.Bootstrap.Application.UseCases.Configurations.Queries;
using Adasit.Bootstrap.WebApi.Controllers.Base;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

[ApiController]
[Route("[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class ConfigurationsController : BaseController
{
    private readonly IMediator mediator;

    private readonly ILogger<ConfigurationsController> _logger;

    public ConfigurationsController(ILogger<ConfigurationsController> logger, IMediator mediator, Notifier notifier) : base(notifier)
    {
        _logger = logger;

        this.mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(DefaultResponseDto<ConfigurationOutputDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DefaultResponseDto<>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(DefaultResponseDto<>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(
        [FromBody] RegisterConfigurationInputDto apiDto,
        CancellationToken cancellationToken
    )
    {
        if (apiDto == null)
        {
            return UnprocessableEntity(new DefaultResponseDto<object>());
        }

        var entity = new RegisterConfigurationInput(apiDto);

        var output = await mediator.Send(entity, cancellationToken);

        return Result(output);
    }

    [HttpPatch("{id:guid}")]
    [ProducesResponseType(typeof(DefaultResponseDto<ConfigurationOutputDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DefaultResponseDto<>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(DefaultResponseDto<>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> PatchConfiguration(
        [FromBody] JsonPatchDocument<ModifyConfigurationInputDto> apiDto,
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        if(apiDto == null)
        {
            return UnprocessableEntity(new DefaultResponseDto<object>());
        }

        CheckIdIfIdIsNull(id);

        if (notifier.Erros.Any())
        {
            return Result<ConfigurationOutputDto>(null!);
        }

        var input = apiDto.MapDtoToInput();

        var entity = new PatchConfiguration(id, input);

        var output = await mediator.Send(entity, cancellationToken);

        return Result(output);
    }

    
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(DefaultResponseDto<ConfigurationOutputDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DefaultResponseDto<>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(DefaultResponseDto<>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update(
        [FromBody] ModifyConfigurationInputDto apiInput,
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        CheckIdIfIdIsNull(id);

        if (notifier.Erros.Any())
        {
            return Result<ConfigurationOutputDto>(null!);
        }

        var input = new ModifyConfigurationInput(apiInput)
        {
            Id = id
        };

        var output = await mediator.Send(input, cancellationToken);
        
        return Result(output);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(DefaultResponseDto<>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DefaultResponseDto<>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        CheckIdIfIdIsNull(id);

        if (notifier.Erros.Any())
        {
            return Result<ConfigurationOutputDto>(null!);
        }

        await mediator.Send(new RemoveConfigurationInput(id), cancellationToken);

        return Result<ConfigurationOutputDto>(null!);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(DefaultResponseDto<ConfigurationOutputDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DefaultResponseDto<>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        CheckIdIfIdIsNull(id);

        if (notifier.Erros.Any())
        {
            return Result<ConfigurationOutputDto>(null!);
        }

        var output = await mediator.Send(new GetConfigurationInput(id), cancellationToken);

        return Result(output);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ListConfigurationsOutput), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        CancellationToken cancellationToken,
        [FromQuery] int? page = null,
        [FromQuery(Name = "per_page")] int? perPage = null,
        [FromQuery] string? search = null,
        [FromQuery] string? sort = null,
        [FromQuery] SearchOrder? dir = null
    )
    {
        var input = new ListConfigurationsInput();
        if (page is not null) input.Page = page.Value;
        if (perPage is not null) input.PerPage = perPage.Value;
        if (!String.IsNullOrWhiteSpace(search)) input.Search = search;
        if (!String.IsNullOrWhiteSpace(sort)) input.Sort = sort;
        if (dir is not null) input.Dir = dir.Value;

        var output = await mediator.Send(input, cancellationToken);
        return Result(output);
    }
}
