using Adasit.Bootstrap.Application.Dto.Models.Errors;

namespace Adasit.Bootstrap.Application.Dto.Models;
public class DefaultResponseDto<T> where T : class
{
    public DefaultResponseDto()
    {
        Errors = new List<ErrorModel>();
        Warnings = new List<ErrorModel>();
    }

    public DefaultResponseDto(T data)
    {
        Errors = new List<ErrorModel>();
        Data = data;
        Warnings = new List<ErrorModel>();
    }

    public DefaultResponseDto(T data, List<ErrorModel> errors)
    {
        Errors = new List<ErrorModel>();

        Data = data;

        if (errors.Any())
        {
            Errors.AddRange(errors);
        }

        Warnings = new List<ErrorModel>();
    }

    public T Data { get; set; }
    public List<ErrorModel> Errors { get; set; }
    public List<ErrorModel> Warnings { get; set; }
}
