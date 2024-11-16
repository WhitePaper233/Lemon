namespace Lemon.Token.Store;

public class TokenInfo
{
    public required string Token { get; set; }
    public required DateTime ExpireTime { get; set; }
}