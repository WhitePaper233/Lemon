namespace Lemon.Token;

public static class Constants
{
    public static class JWTToken
    {
        public const int MaxTokenCount = 5;
        public const int TokenExpireTime = 7 * 24; // 7 days
        public const string Issuer = "lemon.whitepaper233.top";
        public const string Subject = "JWT Token";
        public const string Secret = "Emilia.My.Waifu;Emilia.My.Love;EMT";
    }
}