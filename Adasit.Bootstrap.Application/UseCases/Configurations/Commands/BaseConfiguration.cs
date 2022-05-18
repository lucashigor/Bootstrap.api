namespace Adasit.Bootstrap.Application.UseCases.Configurations.Commands;
using System;

public class BaseConfiguration
{
    public string Name { get; set; }
    public string Value { get; set; }
    public string Description { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset FinalDate { get; set; }

    public BaseConfiguration()
    {
        this.Name = string.Empty;
        this.Value = string.Empty;
        this.Description = string.Empty;
    }
}
