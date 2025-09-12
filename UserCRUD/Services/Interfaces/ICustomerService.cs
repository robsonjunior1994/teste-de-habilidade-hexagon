using UserCRUD.Common;
using UserCRUD.DTOs.Request;
using UserCRUD.Models;

namespace UserCRUD.Services.Interfaces
{
    public interface ICustomerService
    {
        public Task<Result<Customer>> Create(CustomerRequestDTO customerDTO, string email);
    }
}
