namespace Adasit.Bootstrap.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Mime;

public class AuthenticationAttribute : Attribute, IAsyncAuthorizationFilter, IAuthorizationFilter, IOrderedFilter
{
    private readonly string authenticationScheme = "apiKey";
    private readonly List<string> key = new () { "836041c5-936a-4978-b3c2-f8e5b8c01445" };

    int IOrderedFilter.Order => 0;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        OnAuthorizationAsync(context).Wait();
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var req = context.HttpContext.Request;


        string value = req.Headers[authenticationScheme];

        if (String.IsNullOrEmpty(value) || !key.Contains(value))
        {
            context.Result = new UnauthorizedResult();
            context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
            context.HttpContext.Response.ContentType = MediaTypeNames.Application.Json;
            await context.HttpContext.Response.WriteAsync("{\"data\": null,\"httpStatusCode\": 401}");
        }
    }
}
