namespace Adasit.Bootstrap.TestsUtil;
using AutoFixture;
using Bogus;

public class BaseFixture
{
    protected Fixture fixture;
    protected Faker Faker { get; set; }

    public BaseFixture()
    {
        fixture = new();
        Faker = new();
    }
    public string GetStringRigthSize(int minLength, int maxlength)
    {
        var stringValue = Faker.Lorem.Random.Words(2);

        while (stringValue.Length < minLength)
        {
            stringValue += Faker.Lorem.Random.Words(2);
        }

        if (stringValue.Length > maxlength)
        {
            stringValue = stringValue[..maxlength];
        }

        return stringValue;
    }
}