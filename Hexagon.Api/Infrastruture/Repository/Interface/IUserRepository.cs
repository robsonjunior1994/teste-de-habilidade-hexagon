using Hexagon.Api.Domain.Models;

namespace Hexagon.Api.Infrastruture.Repository.Interface
{
    public interface IUserRepository
    {
        public Task Create(User user);
        public Task<User> GetByEmail(string email);
    }
}
