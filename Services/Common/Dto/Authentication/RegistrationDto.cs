using System.ComponentModel.DataAnnotations;

namespace Services.Common.Dto.Authentication;

public class RegistrationDto
{
    public string? FirstName { get; set; } = null!;
    public string? LastName { get; set; } = null!;
    public string? Email { get; set; }

    [Required(ErrorMessage = "Required field")]
    [RegularExpression(@"(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).*",
    ErrorMessage = "Password must be at least 7 characters long and contain at least one uppercase letter and one number.")]
    [StringLength(16, MinimumLength = 7, ErrorMessage = "At least 7 characters")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Required field")]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = null!;

    public string? MobileNumber { get; set; } = null!;
    public string? Role { get; set; }
}
