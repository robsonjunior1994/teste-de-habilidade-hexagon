using UserCRUD.Models;

namespace UserCRUD.Repository.Interface
{
    public interface ICustomerRepository
    {
        public Task Create(Customer customer);
        public Task<Customer> GetByCpfAndUserId(string cpf, int userId);
    }
}
