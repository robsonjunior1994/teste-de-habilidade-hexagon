using Hexagon.Api.Common;
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
            catch 
            {
                // CRIAR SERVIÇO DE LOG
                return Result<Customer>.Failure("An error occurred while creating the customer.", ErrorCode.DATABASE_ERROR);
            }
        }

        public async Task<Result<PaginatedResult<Customer>>> GetAll(int id, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var result = await _customerRepository.GetAll(id, pageNumber, pageSize);

                var paginatedResult = new PaginatedResult<Customer>
                {
                    Items = result.Items,
                    TotalCount = result.TotalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling(result.TotalCount / (double)pageSize)
                };

                return Result<PaginatedResult<Customer>>.Success(paginatedResult);
            }
            catch 
            {
                // CRIAR SERVIÇO DE LOG
                return Result<PaginatedResult<Customer>>.Failure("An error occurred while retrieving customers.", ErrorCode.DATABASE_ERROR);
            }
        }

        public async Task<Result<Customer>> GetForId(int userId, int customerId)
        {
            try
            {
                var customer = await _customerRepository.GetForId(userId, customerId);
                if (customer == null)
                {
                    return Result<Customer>.Failure("Customer not found.", ErrorCode.RESOURCE_NOT_FOUND);
                }
                return Result<Customer>.Success(customer);
            }
            catch 
            {
                // CRIAR SERVIÇO DE LOG
                return Result<Customer>.Failure("An error occurred while retrieving the customer.", ErrorCode.DATABASE_ERROR);
            }
        }

        public async Task<Result<Customer>> DeleteForId(int userId, int customerId)
        {
            try
            {
                var customer = await _customerRepository.GetForId(userId, customerId);
                if (customer == null)
                {
                    return Result<Customer>.Failure("Customer not found.", ErrorCode.RESOURCE_NOT_FOUND);
                }
                await _customerRepository.Delete(customer);
                return Result<Customer>.Success(customer);
            }
            catch 
            {
                // CRIAR SERVIÇO DE LOG
                return Result<Customer>.Failure("An error occurred while deleting the customer.", ErrorCode.DATABASE_ERROR);
            }
        }
        public async Task<Result<Customer>> Update(int userId, int customerId, CustomerRequestDTO customerForUpdate)
        {
            try
            {
                var customer = await _customerRepository.GetForId(userId, customerId);
                if (customer == null)
                {
                    return Result<Customer>.Failure("Customer not found.", ErrorCode.RESOURCE_NOT_FOUND);
                }

                customer.Name = customerForUpdate.Name;
                customer.Age = customerForUpdate.Age;
                customer.CivilState = customerForUpdate.CivilState;
                customer.Cpf = customerForUpdate.Cpf;
                customer.City = customerForUpdate.City;
                customer.State = customerForUpdate.State;

                await _customerRepository.Update(customer);
                return Result<Customer>.Success(customer);
            }
            catch 
            {
                // CRIAR SERVIÇO DE LOG
                return Result<Customer>.Failure("An error occurred while updating the customer.", ErrorCode.DATABASE_ERROR);
            }
        }
    }
}
