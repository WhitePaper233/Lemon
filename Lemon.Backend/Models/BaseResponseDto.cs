namespace Lemon.Backend.Models;

public class BaseResponseDto
{
    public int Code { get; set; }
    public string Message { get; set; } = "";

    public BaseResponseDto(int code, string message)
    {
        Code = code;
        Message = message;
    }
}

public class BaseResponseWithData<T>(int code, string message, T? data) : BaseResponseDto(code, message)
{
    public T? Data { get; set; } = data;
}
