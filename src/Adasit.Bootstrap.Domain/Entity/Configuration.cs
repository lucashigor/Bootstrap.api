namespace Adasit.Bootstrap.Domain.Entity;

using Adasit.Bootstrap.Domain.Conts;
using Adasit.Bootstrap.Domain.Exceptions;
using SeedWork;
using Validation;

public class Configuration : AgregateRoot
{
    public string Name { get; private set; }
    public string Value { get; private set; }
    public string Description { get; private set; }
    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset FinalDate { get; private set; }

    public Configuration(string name,
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

        Validate();
    }

    protected override void Validate()
    {
        AddNotification(Name.NotNullOrEmptyOrWhiteSpace());
        AddNotification(Name.BetweenLength(3, 100));

        AddNotification(Value.NotNullOrEmptyOrWhiteSpace());
        AddNotification(Value.BetweenLength(3, 1000));

        AddNotification(Description.NotNullOrEmptyOrWhiteSpace());
        AddNotification(Description.BetweenLength(3, 1000));

        AddNotification(StartDate.NotDefaultDateTime());

        AddNotification(FinalDate.NotDefaultDateTime());

        base.Validate();
    }

    public void Modify(string name,
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

        LastUpdateAt = DateTimeOffset.UtcNow;

        Validate();
    }

    public void FixStartDate()
    {
        if (StartDate < DateTimeOffset.UtcNow)
        {
            StartDate = DateTimeOffset.UtcNow;
        }

        Validate();
    }

    public void FixFinalDate()
    {
        if (FinalDate > DateTimeOffset.UtcNow)
        {
            FinalDate = DateTimeOffset.UtcNow;
        }

        Validate();
    }
}