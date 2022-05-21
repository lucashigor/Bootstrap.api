namespace Adasit.Bootstrap.Application.Dto.Models.Response;

public class ConfigurationOutputDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public string Description { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset FinalDate { get; set; }

    public ConfigurationOutputDto()
    {

    }

    public ConfigurationOutputDto(Guid id, string name, string value, string description, DateTimeOffset startDate, DateTimeOffset finalDate)
    {
        Id = id;
        Name = name;
        Value = value;
        Description = description;
        StartDate = startDate;
        FinalDate = finalDate;
    }
}
