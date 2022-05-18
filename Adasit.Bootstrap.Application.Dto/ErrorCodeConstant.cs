namespace Adasit.Bootstrap.Application.Dto;
using Adasit.Bootstrap.Application.Dto.Models;

public static class ErrorCodeConstant
{
    public static ErrorModel Generic() => new("0001", "Unfortunately an error occurred during the processing.");
    public static ErrorModel Validation() => new("0002", "Unfortunately your request do not pass in our validation process.");
    public static ErrorModel ErrorOnSavingNewConfiguration() => new("0003", "Unfortunately an error occorred when saving the Configuration.");
    public static ErrorModel NotificationValuesError() => new("0004", "Error on creating a notification.");
    public static ErrorModel ThereWillCurrentConfigurationStartDate() => new ("0005", "There will be a current configuration on the start date.");
    public static ErrorModel ThereWillCurrentConfigurationEndDate() => new("0006", "There will be a current configuration on the end date.");
    public static ErrorModel StartDateCannotBeBeforeToToday() => new("0007", "The start date cannot be before today.");
    public static ErrorModel EndDateCannotBeBeforeToToday() => new("0008", "The end date cannot be before today.");
    public static ErrorModel InvalidOperationOnPatch() => new("0009", "This operation are not valid on patch.");
    public static ErrorModel InvalidPathOnPatch() => new("0010", "This path cannot be changed on patch.");
    public static ErrorModel ConfigurationNotFound() => new("0011", "Configuration Not Found.");
    public static ErrorModel OnlyDescriptionAreAvaliableToChangedOnClosedConfiguration() => new("0012", "Only description are avaliable to be changed on closed configuration.");
    public static ErrorModel ItsNotAllowedToChangeFinalDatetoBeforeToday() => new("0013", "It's not allowed to change final date to before today.");
    public static ErrorModel ItsNotAllowedToChangeInitialDate() => new("0014", "It's not allowed to change initial date on configurations in course.");
    public static ErrorModel ItsNotAllowedToChangeName() => new("0015", "It's not allowed to change name on configurations in course.");
    public static ErrorModel TheMinimunDurationIsOneHour() => new("0016", "The minimun duration is one hour.");
}
