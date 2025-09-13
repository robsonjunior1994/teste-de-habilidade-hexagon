using System.ComponentModel.DataAnnotations;

namespace Hexagon.Api.Presentation.DTOs.Request
{
    public class LoginRequestDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
