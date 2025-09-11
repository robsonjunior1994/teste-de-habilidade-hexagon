using UserCRUD.Common;
using UserCRUD.DTOs;
using UserCRUD.Models;

namespace UserCRUD.Services.Interfaces
{
    public interface IUserService
    {
        public Task<Result<User>> Create(UserDTO userDTO);
    }
}
