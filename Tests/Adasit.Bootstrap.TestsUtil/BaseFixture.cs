namespace Adasit.Bootstrap.TestsUtil;
using AutoFixture;
public class BaseFixture
{
    protected Fixture fixture;

    public BaseFixture()
    {
        fixture = new();
    }
    public string GetStringRigthSize(int minLength, int maxlength)
    {
        var stringValue = "";

        while (stringValue.Length < minLength)
        {
            stringValue += fixture.Create<string>();
        }

        if (stringValue.Length > maxlength)
        {
            stringValue = stringValue[..maxlength];
        }

        return stringValue;
    }
}