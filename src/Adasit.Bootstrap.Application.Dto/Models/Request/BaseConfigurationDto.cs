namespace Adasit.Bootstrap.Application.Dto.Models.Request;

public abstract class BaseConfigurationDto
{
    public string Name { get; set; }
    public string Value { get; set; }
    public string Description { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset FinalDate { get; set; }

    public BaseConfigurationDto()
    {
        Name = "";
        Value = "";
        Description = "";
    }

    public BaseConfigurationDto(string name,
        string value,
        string description,
        DateTimeOffset startDate,
        DateTimeOffset finalDate)
    {
        Name = name;
        Value = value;
        Description = description;
        StartDate = startDate;
        FinalDate = finalDate;
    }
}
