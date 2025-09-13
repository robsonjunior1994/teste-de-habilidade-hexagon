using Hexagon.Api.Domain.Models;
using Hexagon.Api.Infrastruture.Data;
using Hexagon.Api.Infrastruture.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Hexagon.Api.Infrastruture.Repository
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

        public async Task<(List<Customer> Items, int TotalCount)> GetAll(int userId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var query = _context.Customers
                    .Where(c => c.User.Id == userId);

                var totalCount = await query.CountAsync();

                var items = await query
                    .OrderBy(c => c.Name)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (items, totalCount);
            }
            catch
            {
                // Log do erro aqui
                throw;
            }
        }

        public async Task<Customer> GetForId(int userId, int customerId)
        {
            try
            {
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Id == customerId && c.User.Id == userId);

                return customer;
            }
            catch
            {
                // Log do erro aqui
                throw;
            }
        }
        public async Task Delete(Customer customer)
        {
            try
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
            catch
            {
                // Log do erro aqui
                throw;
            }
        }
        public async Task Update(Customer customer)
        {
            try
            {
                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();
            }
            catch
            {
                // Log do erro aqui
                throw;
            }
        }


    }
}
