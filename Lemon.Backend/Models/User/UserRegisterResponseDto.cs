namespace Lemon.Backend.Models.User;

public class UserRegisterResponseDto : BaseResponseDto
{
    UserRegisterResponseDto(int code, string message) : base(code, message) { }

    public static UserRegisterResponseDto Success(string message)
    {
        return new UserRegisterResponseDto(200, message);
    }

    public static UserRegisterResponseDto Fail(string message)
    {
        return new UserRegisterResponseDto(400, message);
    }
}
