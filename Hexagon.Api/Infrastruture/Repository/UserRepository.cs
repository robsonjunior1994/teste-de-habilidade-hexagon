using Hexagon.Api.Domain.Models;
using Hexagon.Api.Infrastruture.Data;
using Hexagon.Api.Infrastruture.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Hexagon.Api.Infrastruture.Repository
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
            try {
                return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception)
            {
                // Log do erro aqui
                throw;
            }
            
        }

    }
}
