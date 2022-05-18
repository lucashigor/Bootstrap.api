namespace Adasit.Bootstrap.Application.UseCases.Configurations.Commands;
using System;
using Adasit.Bootstrap.Application.Dto.Models.Request;
using Adasit.Bootstrap.Application.Dto.Models.Response;
using MediatR;

public class ModifyConfigurationInput : BaseConfiguration, IRequest<ConfigurationOutputDto>
{
    public Guid Id { get; set; }
    public ModifyConfigurationInput() : base()
    { }

    public ModifyConfigurationInput(ModifyConfigurationInputDto dto)
    {
        this.Name = dto.Name;
        this.Value = dto.Value;
        this.Description = dto.Description;
        this.StartDate = dto.StartDate;
        this.FinalDate = dto.FinalDate;
    }

    public ModifyConfigurationInput(Guid Id, string name, string value, string description, DateTimeOffset startDate, DateTimeOffset finalDate)
    {
        this.Id = Id;
        this.Name = name;
        this.Value = value;
        this.Description = description;
        this.StartDate = startDate;
        this.FinalDate = finalDate;
    }
}
