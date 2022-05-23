
namespace Adasit.Bootstrap.Application.Dto.Models.Errors;
public enum ErrorCodes
{
    Generic = 10001,
    Validation = 10002,
    ErrorOnSavingNewConfiguration = 10003,
    NotificationValuesError = 10004,
    ThereWillCurrentConfigurationStartDate = 10005,
    ThereWillCurrentConfigurationEndDate = 10006,
    StartDateCannotBeBeforeUtcNow = 10007,
    EndDateCannotBeBeforeToToday = 10008,
    InvalidOperationOnPatch = 10009,
    InvalidPathOnPatch = 10010,
    ConfigurationNotFound = 10011,
    OnlyDescriptionAreAvaliableToChangedOnClosedConfiguration = 10012,
    ItsNotAllowedToChangeFinalDatetoBeforeToday = 10013,
    ItsNotAllowedToChangeInitialDate = 10014,
    ItsNotAllowedToChangeName = 10015,
    TheMinimunDurationIsOneHour = 10016,
    ConfigurationInCourse = 10017,
    ThisCannotBeDoneOnClosedConfiguration = 10018,
    UnavailableFeatureFlag = 10019,
    ClientHttp = 10020
}
