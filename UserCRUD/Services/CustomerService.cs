using UserCRUD.Common;
using UserCRUD.DTOs.Request;
using UserCRUD.Models;
using UserCRUD.Repository.Interface;
using UserCRUD.Services.Interfaces;

namespace UserCRUD.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserRepository _userRepository;
        public CustomerService(ICustomerRepository customerRepository, IUserRepository userRepository)
        {
            _customerRepository = customerRepository;
            _userRepository = userRepository;
        }
        public async Task<Result<Customer>> Create(CustomerRequestDTO customerDTO, string email)
        {
            var user = await _userRepository.GetByEmail(email);

            var customer = await _customerRepository.GetByCpfAndUserId(customerDTO.Cpf, user.Id);

            if (customer != null)
            {
                return Result<Customer>.Failure("Customer already registered", ErrorCode.RESOURCE_ALREADY_EXISTS);
            }            

            var newCustomer = new Customer
            {
                Name = customerDTO.Name,
                Age = customerDTO.Age,
                CivilState = customerDTO.CivilState,
                Cpf = customerDTO.Cpf,
                City = customerDTO.City,
                State = customerDTO.State,
                User = user
            };

            try
            {
                await _customerRepository.Create(newCustomer);
                return Result<Customer>.Success(newCustomer);
            }
            catch (Exception ex)
            {
                // CRIAR SERVIÇO DE LOG
                return Result<Customer>.Failure("An error occurred while creating the customer.", ErrorCode.DATABASE_ERROR);
            }
        }
    }
}
