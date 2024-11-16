namespace Lemon.Backend.Models.User;

using System.ComponentModel.DataAnnotations;

public class UserLoginDto
{
    [RegularExpression(Utils.RegularExpressions.UserName.Pattern, ErrorMessage = Utils.RegularExpressions.UserName.Tips)]
    public string? UserName { get; set; } = null;

    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string? Email { get; set; } = null;

    [Phone(ErrorMessage = "Invalid Phone Number")]
    public string? PhoneNumber { get; set; } = null;

    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; set; }

    public bool IsValid()
    {
        return !string.IsNullOrEmpty(UserName) || !string.IsNullOrEmpty(Email) || !string.IsNullOrEmpty(PhoneNumber);
    }
}