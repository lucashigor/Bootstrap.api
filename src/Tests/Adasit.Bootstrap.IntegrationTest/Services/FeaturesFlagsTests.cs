namespace Adasit.Bootstrap.IntegrationTest.Services;

using Adasit.Bootstrap.Application.Models.FeatureFlag;
using Adasit.Bootstrap.Infrastructure.Services.FeatureFlag;
using Adasit.Bootstrap.TestsUtil;
using FluentAssertions;
using Refit;

[Collection(nameof(ConfigurationTestFixture))]
public class FeaturesFlagsTests
{
    private readonly ConfigurationTestFixture fixture;

    public FeaturesFlagsTests(ConfigurationTestFixture fixture)
    {
        this.fixture = fixture;
        BaseFixture.CreateWireMock();
    }

    [Fact(DisplayName = nameof(GetFeatureFlagWithSuccess))]
    [Trait("Integration", "FeatureFlag - ")]
    public async void GetFeatureFlagWithSuccess()
    {
        //Arrange
        var req = new RequestDto("geral", CurrentFeatures.FeatureFlagToTest, new());

        BaseFixture.MockFeatureFlag(req, "on");

        IFeatureFlagClient _api;

        _api = RestService.For<IFeatureFlagClient>("http://localhost:9999/infra/featureflag/v1/");

        var app = new FeatureFlagService(_api);

        //Act
        var ret = await app.IsEnabledAsync("geral", CurrentFeatures.FeatureFlagToTest);

        //Assert
        ret.Should().BeTrue();
    }
}
