using Microsoft.EntityFrameworkCore;
using UserCRUD.Data;
using UserCRUD.Models;
using UserCRUD.Repository.Interface;

namespace UserCRUD.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Create(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                // Log do erro aqui
                throw;
            }
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

    }
}
