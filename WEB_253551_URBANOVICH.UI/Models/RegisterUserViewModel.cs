using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace WEB_253551_URBANOVICH.UI.Models;

public class RegisterUserViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required, Compare(nameof(Password))]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;

    public IFormFile? Avatar { get; set; }
}
