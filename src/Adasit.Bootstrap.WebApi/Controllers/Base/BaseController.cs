using Adasit.Bootstrap.Application.Dto.Models;
using Adasit.Bootstrap.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Adasit.Bootstrap.WebApi.Controllers.Base;

public class BaseController : ControllerBase
{
    private readonly Notifier notifier;

    public BaseController(Notifier notifier)
    {
        this.notifier = notifier;
    }

    public IActionResult Result(object model)
    {
        DefaultResponseDto<object> responseDto = new();

        responseDto.Data = model;

        if (notifier.Warnings.Any())
        {
            responseDto.Warnings.AddRange(notifier.Warnings);
        }

        if (notifier.Erros.Any())
        {
            responseDto.Errors.AddRange(notifier.Erros);
        
            return BadRequest(responseDto);
        }

        return Ok(responseDto);
    }
}