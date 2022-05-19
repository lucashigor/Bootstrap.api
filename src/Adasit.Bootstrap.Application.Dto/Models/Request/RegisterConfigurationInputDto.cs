namespace Adasit.Bootstrap.Application.Dto.Models.Request;

public class RegisterConfigurationInputDto : BaseConfigurationDto
{
    public RegisterConfigurationInputDto(string name,
        string value,
        string description,
        DateTimeOffset startDate,
        DateTimeOffset finalDate) : base(name,value,description,startDate,finalDate)
    {

    }

    public RegisterConfigurationInputDto() : base()
    {
    }
}
