using UserCRUD.Common;
using UserCRUD.DTOs.Request;
using UserCRUD.Models;

namespace UserCRUD.Services.Interfaces
{
    public interface ICustomerService
    {
        public Task<Result<Customer>> Create(CustomerRequestDTO customerDTO, string email);
        Task<Result<Customer>> DeleteForId(int v, int customerId);
        public Task<Result<List<Customer>>> GetAll(int id);
        public Task<Result<Customer>> GetForId(int v, int customerId);
        Task<Result<Customer>> Update(int v, int customerId, CustomerRequestDTO customerForUpdate);
    }
}
