using System.Net;
using System.Net.Mime;
using Adasit.Bootstrap.Application.Dto.Models.Errors;

namespace Adasit.Bootstrap.Application.Dto;

public class BusinessException : Exception
{
    public ErrorModel ErrorCode { get; set; }
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.InternalServerError;
    public string ContentType { get; set; } = MediaTypeNames.Application.Json;

    public BusinessException(ErrorModel errorCode) : base(errorCode.Message)
    {
        ErrorCode = errorCode;
    }

    public BusinessException(ErrorModel errorCode, Exception inner) : base(errorCode.Message, inner)
    {
        ErrorCode = errorCode;
    }
}
