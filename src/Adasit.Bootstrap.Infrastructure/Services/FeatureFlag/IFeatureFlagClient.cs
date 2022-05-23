namespace Adasit.Bootstrap.Infrastructure.Services.FeatureFlag;

using Adasit.Bootstrap.Application.Dto.Models;
using Adasit.Bootstrap.Application.Models.FeatureFlag;
using Refit;
using System.Threading.Tasks;

public interface IFeatureFlagClient
{
    [Post("/flags")]
    Task<DefaultResponseDto<string>> Flags([Body] RequestDto requestDto);
}
