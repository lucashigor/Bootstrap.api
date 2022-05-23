namespace Adasit.Bootstrap.Application.Dto.Models.Errors;
using Adasit.Bootstrap.Application.Dto.Models;

public static class ErrorCodeConstant
{
    public static ErrorModel Generic() => new(ErrorCodes.Generic, "Unfortunately an error occurred during the processing.");
    public static ErrorModel UnavailableFeatureFlag() => new(ErrorCodes.UnavailableFeatureFlag, "Unavailable FeatureFlag.");
    public static ErrorModel ClientHttp() => new(ErrorCodes.ClientHttp, "Client Http error.");
    public static ErrorModel Validation() => new(ErrorCodes.Validation, "Unfortunately your request do not pass in our validation process.");
    public static ErrorModel ErrorOnSavingNewConfiguration() => new(ErrorCodes.ErrorOnSavingNewConfiguration, "Unfortunately an error occorred when saving the Configuration.");
    public static ErrorModel NotificationValuesError() => new(ErrorCodes.NotificationValuesError, "Error on creating a notification.");
    public static ErrorModel ThereWillCurrentConfigurationStartDate() => new(ErrorCodes.ThereWillCurrentConfigurationStartDate, "There will be a current configuration on the start date.");
    public static ErrorModel ThereWillCurrentConfigurationEndDate() => new(ErrorCodes.ThereWillCurrentConfigurationEndDate, "There will be a current configuration on the end date.");
    public static ErrorModel StartDateCannotBeBeforeToUtcNow() => new(ErrorCodes.StartDateCannotBeBeforeUtcNow, "The start date cannot be before utc now.");
    public static ErrorModel EndDateCannotBeBeforeToToday() => new(ErrorCodes.EndDateCannotBeBeforeToToday, "The end date cannot be before today.");
    public static ErrorModel InvalidOperationOnPatch() => new(ErrorCodes.InvalidOperationOnPatch, "This operation are not valid on patch.");
    public static ErrorModel InvalidPathOnPatch() => new(ErrorCodes.InvalidPathOnPatch, "This path cannot be changed on patch.");
    public static ErrorModel ConfigurationNotFound() => new(ErrorCodes.ConfigurationNotFound, "Configuration Not Found.");
    public static ErrorModel OnlyDescriptionAreAvaliableToChangedOnClosedConfiguration() => new(ErrorCodes.OnlyDescriptionAreAvaliableToChangedOnClosedConfiguration, "Only description are avaliable to be changed on closed configuration.");
    public static ErrorModel ItsNotAllowedToChangeFinalDatetoBeforeToday() => new(ErrorCodes.ItsNotAllowedToChangeFinalDatetoBeforeToday, "It's not allowed to change final date to before today.");
    public static ErrorModel ItsNotAllowedToChangeInitialDate() => new(ErrorCodes.ItsNotAllowedToChangeInitialDate, "It's not allowed to change initial date on configurations in course.");
    public static ErrorModel ItsNotAllowedToChangeName() => new(ErrorCodes.ItsNotAllowedToChangeName, "It's not allowed to change name on configurations in course.");
    public static ErrorModel TheMinimunDurationIsOneHour() => new(ErrorCodes.TheMinimunDurationIsOneHour, "The minimun duration is one hour.");
    public static ErrorModel ConfigurationInCourse() => new(ErrorCodes.ConfigurationInCourse, "This configuration already initiated, so the can't be deleted.");
    public static ErrorModel ThisCannotBeDoneOnClosedConfiguration() => new(ErrorCodes.ThisCannotBeDoneOnClosedConfiguration, "This cannot be done on closed configuration.");
}
