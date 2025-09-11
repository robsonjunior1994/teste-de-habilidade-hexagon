using System.ComponentModel.DataAnnotations;
using UserCRUD.Models;

namespace UserCRUD.DTOs.Response
{
    public class UserResponseDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public UserResponseDTO() { }

        public UserResponseDTO(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
        }
    }
}
