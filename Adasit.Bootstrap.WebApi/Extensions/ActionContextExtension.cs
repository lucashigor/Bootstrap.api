using Adasit.Bootstrap.Application.Dto;
using Adasit.Bootstrap.Application.Dto.Models;
using Microsoft.AspNetCore.Mvc;

namespace Adasit.Bootstrap.WebApi.Extensions;

public static class ActionContextExtension
{
    public static BadRequestObjectResult GetErrorsModelState(this ActionContext actionContext)
    {
        var ret = new DefaultResponseDto<object>();

        actionContext.ModelState.Values.Where(v => v.Errors.Count > 0)
                .SelectMany(v => v.Errors)
                .Select(v => v.ErrorMessage)
                .ToList().ForEach(x => ret.Errors.Add(new ErrorModel(ErrorCodeConstant.Validation().Code, x)));

        return new BadRequestObjectResult(ret);
    }
}
