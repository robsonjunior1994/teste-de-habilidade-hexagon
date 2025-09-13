using Hexagon.Api.Common;
using UserCRUD.Common;
using UserCRUD.Models;

namespace UserCRUD.Repository.Interface
{
    public interface ICustomerRepository
    {
        public Task Create(Customer customer);
        public Task<Customer> GetByCpfAndUserId(string cpf, int userId);
        public Task<(List<Customer> Items, int TotalCount)> GetAll(int userId, int pageNumber = 1, int pageSize = 10);
        public Task<Customer> GetForId(int v, int customerId);
        Task Delete(Customer customer);
        Task Update(Customer customer);
    }
}
