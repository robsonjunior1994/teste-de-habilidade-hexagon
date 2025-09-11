using UserCRUD.Models;

namespace UserCRUD.Repository.Interface
{
    public interface IUserRepository
    {
        public Task Create(User user);
        public Task<User> GetByEmail(string email);
    }
}
