namespace Lemon.Backend.Models;

public class BaseResponseDto(int code, string message)
{
    public int Code { get; set; } = code;
    public string Message { get; set; } = message;
}

public class BaseResponseWithData<T>(int code, string message, T? data) : BaseResponseDto(code, message)
{
    public T? Data { get; set; } = data;
}
