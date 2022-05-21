namespace Adasit.Bootstrap.Application.Models;
using Adasit.Bootstrap.Application.Dto.Models.Errors;

public class Notifier
{
    public Notifier()
    {
        Warnings = new List<ErrorModel>();
        Erros = new List<ErrorModel>();
    }

    public List<ErrorModel> Warnings { get; private set; }
    public List<ErrorModel> Erros { get; private set; }
}
