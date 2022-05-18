namespace Adasit.Bootstrap.Domain.Exceptions;

using Adasit.Bootstrap.Domain.Conts;

public class EntityGenericException : Exception
{
    private readonly ErrorsCodes _genericCode = ErrorsCodes.Generic;
    public List<ErrorsCodes> Code { get; private set; }

    public EntityGenericException(string? message) : base(message)
    {
        Code = new ();

        Code.Add(_genericCode);
    }

    public EntityGenericException(string? message, ErrorsCodes code) : base(message)
    {
        Code = new ();

        Code.Add(code);
    }

    public EntityGenericException(string? message, List<ErrorsCodes> code) : base(message)
    {
        Code = new ();

        Code.AddRange(code.Distinct());
    }
}