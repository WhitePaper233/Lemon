namespace Lemon.Backend.Models.User;

public class UserProfileResponseDto(int code, string message, UserProfileResponseData? data = null) : BaseResponseWithData<UserProfileResponseData>(code, message, data)
{
    public static UserProfileResponseDto Success(Entities.User user)
    {
        var data = new UserProfileResponseData
        {
            Id = user.Id,
            UserName = user.UserName,
            NickName = user.NickName
        };
        return new UserProfileResponseDto(200, "", data);
    }

    public static UserProfileResponseDto Fail(string message)
    {
        return new UserProfileResponseDto(400, message);
    }
}

public class UserProfileResponseData
{
    public Guid Id { get; set; }
    public required string UserName { get; set; }
    public required string NickName { get; set; }
}
