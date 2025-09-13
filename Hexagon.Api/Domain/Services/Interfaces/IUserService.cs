using Hexagon.Api.Domain.Models;
using Hexagon.Api.Presentation.Common;
using Hexagon.Api.Presentation.DTOs.Request;

namespace Hexagon.Api.Domain.Services.Interfaces
{
    public interface IUserService
    {
        public Task<Result<User>> Create(UserRequestDTO userDTO);
        Task<Result<string>> Login(LoginRequestDTO loginDTO);

    }
}
