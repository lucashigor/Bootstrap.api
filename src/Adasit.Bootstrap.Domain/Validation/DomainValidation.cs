namespace Adasit.Bootstrap.Domain.Validation;

using System.Runtime.CompilerServices;
using Adasit.Bootstrap.Domain.Conts;
using Adasit.Bootstrap.Domain.Exceptions;

public static class DomainValidation
{
    public static Notification? NotNull(this object target,
                              [CallerArgumentExpression("target")] string fieldName = "")
    {
        Notification? notification = null;

        if (target is null)
        {
            var message = ErrorsMessages.NotNull.GetMessage(fieldName);
            notification = new Notification(fieldName, message, ErrorsCodes.Validation);
        }

        return notification;
    }

    public static Notification? NotNull(this Guid target,
                              [CallerArgumentExpression("target")] string fieldName = "")
    {
        Notification? notification = null;

        if (target == Guid.Empty)
        {
            var message = ErrorsMessages.NotNull.GetMessage(fieldName);
            notification = new Notification(fieldName, message, ErrorsCodes.Validation);
        }

        return notification;
    }

    public static Notification? NotNullOrEmptyOrWhiteSpace(this string target,
                              [CallerArgumentExpression("target")] string fieldName = "")
    {
        Notification? notification = null;

        if (string.IsNullOrWhiteSpace(target) || string.IsNullOrEmpty(target))
        {
            var message = ErrorsMessages.NotNull.GetMessage(fieldName);
            notification = new Notification(fieldName, message, ErrorsCodes.Validation);
        }

        return notification;
    }

    public static Notification? NotDefaultDateTime(this DateTimeOffset target,
                              [CallerArgumentExpression("target")] string fieldName = "")
    {
        DateTimeOffset? nullableTarget = target;

        var notification = NotDefaultDateTime(nullableTarget, fieldName);

        return notification;
    }

    public static Notification? NotDefaultDateTime(this DateTimeOffset? target,
                          [CallerArgumentExpression("target")] string fieldName = "")
    {
        Notification? notification = null;
        
        if (target.HasValue && target.Value == default)
        {
            var message = ErrorsMessages.NotDefaultDateTime.GetMessage(fieldName);
            notification = new Notification(fieldName, message, ErrorsCodes.Validation);
        }

        return notification;
    }

    public static Notification? BetweenLength(this string target, int minLength, int maxLength,
                              [CallerArgumentExpression("target")] string fieldName = "")
    {
        Notification? notification = null;

        if (!string.IsNullOrEmpty(target) && (target.Length < minLength || target.Length > maxLength))
        {
            var message = ErrorsMessages.BetweenLength.GetMessage(fieldName, minLength, maxLength);
            notification = new Notification(fieldName, message, ErrorsCodes.Validation);
        }

        return notification;
    }
}