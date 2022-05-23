namespace Adasit.Bootstrap.Infrastructure.Services.FeatureFlag;

using Adasit.Bootstrap.Application.Dto;
using Adasit.Bootstrap.Application.Dto.Models;
using Adasit.Bootstrap.Application.Dto.Models.Errors;
using Adasit.Bootstrap.Application.Interfaces;
using Adasit.Bootstrap.Application.Models.FeatureFlag;
using Newtonsoft.Json;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

public class FeatureFlagService : IFeatureFlagService
{
    private static readonly string On = "on";

    private readonly IFeatureFlagClient client;

    public FeatureFlagService(
        IFeatureFlagClient client)
    {
        this.client = client;
    }

    public async Task<bool> IsEnabledAsync(CurrentFeatures feature)
    {
        return await IsEnabledAsync(feature.Value, feature, new Dictionary<string, string>());
    }

    public async Task<bool> IsEnabledAsync(string key, CurrentFeatures feature)
    {
        return await IsEnabledAsync(key, feature, new Dictionary<string, string>());
    }

    public async Task<bool> IsEnabledAsync(string key, CurrentFeatures feature, Dictionary<string, string> att)
    {
        string treatment = await GetValue(key, feature, att);

        return On.Equals(treatment);
    }

    public async Task<string> GetValueAsync(string key, CurrentFeatures feature)
    {
        return await GetValue(key, feature, new Dictionary<string, string>());
    }

    public async Task<string> GetValue(string key, CurrentFeatures feature, Dictionary<string, string> att)
    {
        var listAtt = new List<KeyValueDto>();

        if(att is null)
        {
            att = new Dictionary<string, string>();
        }

        foreach (var item in att)
        {
            listAtt.Add(new KeyValueDto() { Key = item.Key, Value = item.Value });
        }

        RequestDto requestDto = new (key,feature,listAtt);

        DefaultResponseDto<string> responseDto;

        string value;

        try
        {
            responseDto = await client.Flags(requestDto);

            value = responseDto.Data;
        }
        catch (ApiException ex)
        {
            try
            {
                var errorResponseDto = await ex.GetContentAsAsync<DefaultResponseDto<string>>();

                throw new BusinessException(ErrorCodeConstant.UnavailableFeatureFlag(), ex)
                {
                    StatusCode = ex.StatusCode
                };
            }
            catch (JsonReaderException jsonEx)
            {
                throw new BusinessException(ErrorCodeConstant.ClientHttp(), jsonEx)
                {
                    StatusCode = ex.StatusCode
                };
            }

        }

        return value;
    }
}
