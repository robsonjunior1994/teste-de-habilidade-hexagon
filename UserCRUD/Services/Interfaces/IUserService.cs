using UserCRUD.Common;
using UserCRUD.DTOs.Request;
using UserCRUD.DTOs.Response;
using UserCRUD.Models;

namespace UserCRUD.Services.Interfaces
{
    public interface IUserService
    {
        public Task<Result<User>> Create(UserRequestDTO userDTO);
        Task<Result<string>> Login(LoginRequestDTO loginDTO);

    }
}
