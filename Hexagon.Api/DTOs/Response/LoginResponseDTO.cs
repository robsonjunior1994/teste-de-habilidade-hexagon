using UserCRUD.Models;

namespace UserCRUD.DTOs.Response
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public LoginResponseDTO() { }
        public LoginResponseDTO(string token)
        {
            Token = token;
        }
    }
}
