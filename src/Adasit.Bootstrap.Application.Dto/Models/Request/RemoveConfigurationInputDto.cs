namespace Adasit.Bootstrap.Application.Dto.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class RemoveConfigurationInputDto
{
    public Guid Id { get; private set; }

    public RemoveConfigurationInputDto(Guid Id)
    {
        this.Id = Id;
    }
}