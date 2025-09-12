using Microsoft.EntityFrameworkCore;
using UserCRUD.Data;
using UserCRUD.Models;
using UserCRUD.Repository.Interface;

namespace UserCRUD.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;
        public CustomerRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }
        public async Task Create(Customer customer)
        {
            try
            {
                await _context.Customers.AddAsync(customer);
                await _context.SaveChangesAsync();
            }
            catch
            {
                // Log do erro aqui
                throw;
            }
        }

        public async Task<Customer> GetByCpfAndUserId(string cpf, int userId)
        {
            try
            {
                return await _context.Customers
                    .FirstOrDefaultAsync(c => c.Cpf == cpf && c.User.Id == userId);
            }
            catch
            {
                // Log do erro aqui
                throw;
            }
        }
    }
}
