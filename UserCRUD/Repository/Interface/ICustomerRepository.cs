using UserCRUD.Common;
using UserCRUD.Models;

namespace UserCRUD.Repository.Interface
{
    public interface ICustomerRepository
    {
        public Task Create(Customer customer);
        public Task<Customer> GetByCpfAndUserId(string cpf, int userId);
        public Task<List<Customer>> GetAll(int userId);
        public Task<Customer> GetForId(int v, int customerId);
        Task Delete(Customer customer);
        Task Update(Customer customer);
    }
}
