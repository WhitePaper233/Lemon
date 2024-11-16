namespace Lemon.Backend.Utils;

public static class RegularExpressions
{
    public static class UserName
    {
        public const string Pattern = @"^[a-zA-Z0-9_]{3,16}$";
        public const string Tips = "Username must be 3-16 characters long and can only contain letters, numbers and underline";
    }

    public static class Password
    {
        public const string Pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$";
        public const string Tips = "Password must contain at least 8 characters, including uppercase, lowercase letters and numbers";
    }
}