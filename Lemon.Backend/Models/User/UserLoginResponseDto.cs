namespace Lemon.Backend.Models.User;

public class UserLoginResponseDto(int code, string message, UserLoginInfoDto? data = null) : BaseResponseWithData<UserLoginInfoDto>(code, message, data)
{
    public static UserLoginResponseDto Success(string token)
    {
        return new UserLoginResponseDto(200, "Login Success", new UserLoginInfoDto { Token = token });
    }

    public static UserLoginResponseDto Fail(string message)
    {
        return new UserLoginResponseDto(400, message);
    }
}

public class UserLoginInfoDto
{
    public required string Token { get; set; }
}