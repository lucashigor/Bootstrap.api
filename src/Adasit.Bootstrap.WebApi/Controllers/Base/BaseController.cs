using Adasit.Bootstrap.Application.Dto.Models;
using Adasit.Bootstrap.Application.Dto.Models.Errors;
using Adasit.Bootstrap.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Adasit.Bootstrap.WebApi.Controllers.Base;

public class BaseController : ControllerBase
{
    protected Notifier notifier { get; private set; }

    public BaseController(Notifier notifier)
    {
        this.notifier = notifier;
    }

    protected IActionResult Result<T>(T model) where T : class
    {
        DefaultResponseDto<T> responseDto = new(model);

        if (notifier.Warnings.Any())
        {
            responseDto.Warnings.AddRange(notifier.Warnings);
        }

        if (notifier.Erros.Any())
        {
            responseDto.Errors.AddRange(notifier.Erros);
        
            return BadRequest(responseDto);
        }

        if(model is null && !notifier.Warnings.Any() && !notifier.Erros.Any())
        {
            return NoContent();
        }

        return Ok(responseDto);
    }

    protected void CheckIdIfIdIsNull(Guid id)
    {
        if (id == Guid.Empty)
        {
            var err = ErrorCodeConstant.Validation();

            err.ChangeInnerMessage("Id cannot be null");

            this.notifier.Erros.Add(err);

        }
    }
}