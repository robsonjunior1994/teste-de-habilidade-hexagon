using Hexagon.Api.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Hexagon.Api.Presentation.DTOs.Request
{
    public class UserRequestDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        public UserRequestDTO() { }

        public UserRequestDTO(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
        }
    }
}
