namespace Lemon.Backend.Models.User;

public class UserLoginResponseDto(int code, string message, UserLoginResponseData? data = null) : BaseResponseWithData<UserLoginResponseData>(code, message, data)
{
    public static UserLoginResponseDto Success(string token)
    {
        return new UserLoginResponseDto(200, "Login Success", new UserLoginResponseData { Token = token });
    }

    public static UserLoginResponseDto Fail(string message)
    {
        return new UserLoginResponseDto(400, message);
    }
}

public class UserLoginResponseData
{
    public required string Token { get; set; }
}