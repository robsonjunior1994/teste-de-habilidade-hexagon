using UserCRUD.Models;

namespace UserCRUD.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        bool IsTokenValid(string token);

        // Metodos para extrair informações
        string GetUserIdFromToken(string token);
        string GetUserEmailFromToken(string token);
        string GetUserNameFromToken(string token);
    }
}
