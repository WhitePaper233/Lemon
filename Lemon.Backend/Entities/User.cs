using System.ComponentModel.DataAnnotations;
using Lemon.Backend.Services;

namespace Lemon.Backend.Entities;

/// <summary>
/// 用户
/// </summary>
public class User : BaseEntity
{
    [RegularExpression(Utils.RegularExpressions.UserName.Pattern, ErrorMessage = Utils.RegularExpressions.UserName.Tips)]
    public string? UserName { get; set; } // 用户名

    [Required]
    [Phone(ErrorMessage = "Invalid phone number")]
    public required string PhoneNumber { get; set; } // 电话号码

    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string? Email { get; set; } // 电子邮件地址

    [Required]
    [MaxLength(Utils.Constants.UserName.MaxLength)]
    public required string NickName { get; set; } // 昵称

    [Required]
    public required byte[] PasswordHash { get; set; } // 密码哈希

    [Required]
    public required byte[] Salt { get; set; } // 盐值
}
