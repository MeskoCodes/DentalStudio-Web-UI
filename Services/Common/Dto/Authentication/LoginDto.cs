using System.ComponentModel.DataAnnotations;

namespace Services.Common.Dto.Authentication;

public class LoginDto
{
    [Required(ErrorMessage = "E-Mailadresse wird benötigt!")]
    [EmailAddress(ErrorMessage = "E-Mail hat nicht das richtige Format!")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password Required!")]
    public string? Password { get; set; }
}