namespace Adasit.Bootstrap.ComponentTest.Steps;

using System;
using System.Threading.Tasks;
using Adasit.Bootstrap.Application.Dto.Models.Request;
using Adasit.Bootstrap.Application.Dto.Models.Response;
using Adasit.Bootstrap.ComponentTest.Hooks;
using Adasit.Bootstrap.WebApi;
using FluentAssertions;
using TechTalk.SpecFlow;
using System.Linq;
using Adasit.Bootstrap.ComponentTest.Utils;

[Binding]
public sealed class ConfigurationStepDefinitions : Hook
{
    private RegisterConfigurationInputDto Data;
    private ConfigurationOutputDto Result { get; set; }

    private const string ConfigurationsUrl = "/Configurations";

    public ConfigurationStepDefinitions(CustomWebApplicationFactory<Startup> factory) : base(factory)
    {
        Data = new();
    }

    [Given("a valid configuration")]
    public void GivenAValidConfiguration()
    {
        Data = new(
           baseFixture.GetStringRigthSize(3, 100),
           baseFixture.GetStringRigthSize(3, 1000),
            baseFixture.GetStringRigthSize(3, 1000),
            DateTimeOffset.UtcNow.AddDays(1),
            DateTimeOffset.UtcNow.AddDays(15));
    }

    [When("the configuration are sended to request")]
    public async Task WhenTheConfigurationAreSendedToRequestAsync()
    {
        string url = $"{ConfigurationsUrl}";

        Result = await PostWithValidations<ConfigurationOutputDto>(url, Data);
    }

    [Then(@"the id should not be null")]
    public void ThenTheIdShouldNotBeNull()
    {
        Result?.Id.Should().NotBeEmpty();
    }

    [Then(@"the event '(.*)' has to be sended on this topic '(.*)' for this consumer '(.*)'")]
    public void ThenTheEventHasToBeSendedOnThisTopic(string topicName, 
        string eventName, 
        string fileName)
    {
        message.list.Should().NotBeNull();

        var obj = message.list.Where(x => x.Name.Equals(topicName))
            .FirstOrDefault(x => x.Data.EventName.Equals(eventName));

        PactsGenericValidator.EnsureEventApiHonoursPactWithConsumer(
            microserviceName,
            eventName,
            fileName,
            obj!);
    }
}
