using System.ComponentModel.DataAnnotations;
using Lemon.Backend.Utils;

namespace Lemon.Backend.Models.User;

public class UserRegisterDto
{
    // 手机号
    [Required(ErrorMessage = "PhoneNumber is required")]
    [Phone(ErrorMessage = "Invalid Phone Number")]
    public required string PhoneNumber { get; set; }

    // 邮箱
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string? Email { get; set; } = null;

    // 密码
    [Required(ErrorMessage = "Password is required")]
    [RegularExpression(RegularExpressions.Password.Pattern, ErrorMessage = RegularExpressions.Password.Tips)]
    public required string Password { get; set; }
}
