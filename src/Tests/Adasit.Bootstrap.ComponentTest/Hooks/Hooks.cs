namespace Adasit.Bootstrap.ComponentTest.Hooks;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using Adasit.Bootstrap.Application.Dto.Models;
using Adasit.Bootstrap.Infrastructure.Context;
using Adasit.Bootstrap.TestsUtil;
using Adasit.Bootstrap.WebApi;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TechTalk.SpecFlow;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

[Binding]
public class Hook : IClassFixture<WebApplicationFactory<Startup>>
{
    private CustomWebApplicationFactory<Startup> factory;
    public BaseFixture baseFixture { get; }
    public static WireMockServer WireMockServer { get; set; }
    public static DbContextOptions<PrincipalContext> DbContextOptions { get; set; }

    public Hook(CustomWebApplicationFactory<Startup> factory)
    {
        this.factory = factory;

        this.baseFixture = new();
    }

    [BeforeTestRun]
    public static void BeforeTestRun()
    {
        if (WireMockServer == null || !WireMockServer.IsStarted)
        {
            WireMockServer = WireMockServer.Start(9999);
        }

        DbContextOptions = new DbContextOptionsBuilder<PrincipalContext>()
            .UseInMemoryDatabase("IntegrationTestDatabase")
            .Options;
    }

    [AfterTestRun]
    public static void AfterTestRun()
    {
        WireMockServer?.Stop();
        WireMockServer?.Dispose();
    }

    public void ResetWireMock()
    {
        WireMockServer.Reset();
    }

    public HttpClient CreateAuthorizedClient()
    {
        return CreateAuthorizedClient(null!, null!);
    }
    public HttpClient CreateAuthorizedClient(string token)
    {
        return CreateAuthorizedClient(token, null!);
    }

    public HttpClient CreateAuthorizedClient(string token, string language)
    {
        HttpClient client = factory.CreateClient();

        if (!string.IsNullOrEmpty(language))
        {
            client.DefaultRequestHeaders.Add("Accept-Language", language);
        }
        else
        {
            client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.5");
        }

        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Add("Authorization", token);
        }

        return client;
    }

    public void ResetDatabase()
    {
        using PrincipalContext context = new(DbContextOptions);
        context.Configuration.RemoveRange(context.Configuration);
        context.SaveChanges();

        context.Configuration.AddRange(new List<Domain.Entity.Configuration>()
            {
                new Domain.Entity.Configuration("dfsdfsdfs","dfsdfsdfs fsdfsdfdsfsdfdsfsdfsd","dfsdfsdfs fsdfsdfdsfsdfdsfsdfsd asadasdads",DateTimeOffset.UtcNow.AddDays(1), DateTimeOffset.UtcNow.AddDays(20))
            }
        );

        context.SaveChanges();
    }

    public static IRequestBuilder CreateWiremockRequest(HttpMethod method, string path)
    {
        return CreateWiremockRequest(method, path, new Dictionary<string, string>(), null!);
    }

    public static IRequestBuilder CreateWiremockRequest(HttpMethod method, string path, object body)
    {
        return CreateWiremockRequest(method, path, new Dictionary<string, string>(), body);
    }

    public static IRequestBuilder CreateWiremockRequest(HttpMethod method, string path, Dictionary<string, string> prams)
    {
        return CreateWiremockRequest(method, path, prams, null!);
    }
    public static IRequestBuilder CreateWiremockRequest(HttpMethod method, string path, Dictionary<string, string> prams, object body)
    {
        IRequestBuilder requestBuilder = Request.Create();
        requestBuilder.WithPath(path);

        foreach (KeyValuePair<string, string> keyValue in prams)
        {
            requestBuilder.WithParam(keyValue.Key, keyValue.Value);
        }

        if (body != null)
        {
            var jsonBody = JsonConvert.SerializeObject(body);

            requestBuilder.WithBody(new JsonMatcher(jsonBody));
        }

        return method.Method switch
        {
            "GET" => requestBuilder.UsingGet(),
            "POST" => requestBuilder.UsingPost(),
            "PUT" => requestBuilder.UsingPut(),
            "DELETE" => requestBuilder.UsingDelete(),
            _ => requestBuilder.UsingAnyMethod()
        };
    }

    public static IResponseBuilder CreateWiremockResponse(HttpStatusCode code)
    {
        IResponseBuilder responseBuilder = Response.Create();
        responseBuilder.WithStatusCode(code);
        responseBuilder.WithHeader("Content-Type", MediaTypeNames.Application.Json);

        return responseBuilder;
    }

    public static IResponseBuilder CreateWiremockResponse(HttpStatusCode code, object body)
    {
        IResponseBuilder responseBuilder = CreateWiremockResponse(code);

        responseBuilder.WithBodyAsJson(body);

        return responseBuilder;
    }

    public void RegisterWiremockResponse(IRequestBuilder request, IResponseBuilder response)
    {
        WireMockServer
            .Given(request)
            .RespondWith(response);
    }

    public Task<T> GetWithValidations<T>(string url) where T : class
    {
        return GetWithValidations<T>(url, null, new());
    }

    public async Task<T> GetWithValidations<T>(string url, string code, List<ErrorModel> errorDetails) where T : class
    {
        HttpClient client;

        client = CreateAuthorizedClient();

        var response = await client.GetAsync(url);

        return await ValidateResponse<T>(code, errorDetails, response);
    }

    public Task<T> PostWithValidations<T>(string url, object data) where T : class
    {
        return PostWithValidations<T>(url, null!, new(), data);
    }

    public async Task<T> PostWithValidations<T>(string url, string code, List<ErrorModel> errorDetails, object data) where T : class
    {
        HttpClient client;
        StringContent httpContent;

        client = CreateAuthorizedClient();

        Body(data, out httpContent);

        var response = await client.PostAsync(url, httpContent);

        return await ValidateResponse<T>(code, errorDetails, response);
    }

    private static void Body(object data, out StringContent httpContent)
    {
        string json = string.Empty;

        if (data != null)
        {
            json = JsonConvert.SerializeObject(data);
        }

        httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
    }

    private static async Task<T> ValidateResponse<T>(string code, List<ErrorModel> errorDetails, HttpResponseMessage response) where T : class
    {
        var content = await response.Content.ReadAsStringAsync();
        var ret = JsonConvert.DeserializeObject<DefaultResponseDto<T>>(content);

        ret.Should().NotBeNull();

        Assert.Equal(errorDetails, ret.Errors);

        return ret.Data;
    }

    public void MockFeatureFlag(string ret)
    {
        var response = new DefaultResponseDto<string>(ret);

        RegisterWiremockResponse(
            CreateWiremockRequest(HttpMethod.Post, "/infra/featureflags/v1/flags"),
            CreateWiremockResponse(HttpStatusCode.OK, response));
    }
}
