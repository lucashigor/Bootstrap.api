namespace Adasit.Bootstrap.Application.Interfaces;

using Adasit.Bootstrap.Application.Models.FeatureFlag;
using System.Collections.Generic;
using System.Threading.Tasks;
public interface IFeatureFlagService
{
    Task<string> GetValue(string key, CurrentFeatures feature, Dictionary<string, string> att);
    Task<string> GetValueAsync(string key, CurrentFeatures feature);
    Task<bool> IsEnabledAsync(CurrentFeatures feature);
    Task<bool> IsEnabledAsync(string key, CurrentFeatures feature);
    Task<bool> IsEnabledAsync(string key, CurrentFeatures feature, Dictionary<string, string> att);
}
