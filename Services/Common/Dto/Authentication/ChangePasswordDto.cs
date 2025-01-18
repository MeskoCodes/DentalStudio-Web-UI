using System.ComponentModel.DataAnnotations;

namespace Services.Common.Dto.Authentication;

public class ChangePasswordDto
{
    [Required(ErrorMessage = "Password is required.")]
    public string? OldPassword { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$",
        ErrorMessage =
            "The password must be between 8 and 16 characters long and include at least one uppercase letter, one lowercase letter, one number, and one symbol.")]
    [StringLength(16, MinimumLength = 7, ErrorMessage = "At least 8 characters.")]
    [DataType(DataType.Password)]
    public string? NewPassword { get; set; }

    [Required(ErrorMessage = "Please confirm the password.")]
    [Compare(nameof(NewPassword), ErrorMessage = "Passwords must match.")]
    public string? ConfirmPassword { get; set; }

    public string? Email { get; set; }
}
