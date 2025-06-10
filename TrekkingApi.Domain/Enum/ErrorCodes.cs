
namespace TrekkingApi.Domain.Enum
{
    public enum ErrorCodes
    {

        UserNotFound = 11,
        UserAlreadyExists = 12,

        PasswordNotEqualsPasswordConfirm = 21,
        PasswordIsWrong = 22,

        PinOrMetaNotFound = 30,

        InternalServerError = 44,




    }
}
