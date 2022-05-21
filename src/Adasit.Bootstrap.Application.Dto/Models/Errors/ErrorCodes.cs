
namespace Adasit.Bootstrap.Application.Dto.Models.Errors;
public enum ErrorCodes
{
    Generic = 0001,
    Validation = 0002,
    ErrorOnSavingNewConfiguration = 0003,
    NotificationValuesError = 0004,
    ThereWillCurrentConfigurationStartDate = 0005,
    ThereWillCurrentConfigurationEndDate = 0006,
    StartDateCannotBeBeforeUtcNow = 0007,
    EndDateCannotBeBeforeToToday = 0008,
    InvalidOperationOnPatch = 0009,
    InvalidPathOnPatch = 0010,
    ConfigurationNotFound = 0011,
    OnlyDescriptionAreAvaliableToChangedOnClosedConfiguration = 0012,
    ItsNotAllowedToChangeFinalDatetoBeforeToday = 0013,
    ItsNotAllowedToChangeInitialDate = 0014,
    ItsNotAllowedToChangeName = 0015,
    TheMinimunDurationIsOneHour = 0016,
    ConfigurationInCourse = 0017,
    ThisCannotBeDoneOnClosedConfiguration = 0018
}
