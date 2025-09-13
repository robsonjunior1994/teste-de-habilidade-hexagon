using Moq;
using UserCRUD.Common;
using UserCRUD.DTOs.Request;
using UserCRUD.Models;
using UserCRUD.Repository.Interface;
using UserCRUD.Services;

namespace Hexagon.Tests.Services
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();

            _customerService = new CustomerService(
                _customerRepositoryMock.Object,
                _userRepositoryMock.Object
            );
        }

        [Fact]
        public async Task Create_ShouldReturnSuccess_WhenCustomerDoesNotExist()
        {
            // Arrange
            var customerDto = new CustomerRequestDTO
            {
                Name = "John Doe",
                Age = "30",
                CivilState = "Single",
                Cpf = "12345678900",
                City = "São Paulo",
                State = "SP"
            };

            var userEmail = "test@example.com";
            var user = new User { Id = 1, Email = userEmail };

            _userRepositoryMock.Setup(r => r.GetByEmail(userEmail))
                .ReturnsAsync(user);

            _customerRepositoryMock.Setup(r => r.GetByCpfAndUserId(customerDto.Cpf, user.Id))
                .ReturnsAsync((Customer)null);

            _customerRepositoryMock.Setup(r => r.Create(It.IsAny<Customer>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _customerService.Create(customerDto, userEmail);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(customerDto.Name, result.Data.Name);
            Assert.Equal(customerDto.Cpf, result.Data.Cpf);
            Assert.Equal(user.Id, result.Data.User.Id);
        }

        [Fact]
        public async Task Create_ShouldReturnFailure_WhenCustomerAlreadyExists()
        {
            // Arrange
            var customerDto = new CustomerRequestDTO
            {
                Name = "John Doe",
                Age = "30",
                CivilState = "Single",
                Cpf = "12345678900",
                City = "São Paulo",
                State = "SP"
            };

            var userEmail = "test@example.com";
            var user = new User { Id = 1, Email = userEmail };
            var existingCustomer = new Customer { Cpf = customerDto.Cpf, User = user };

            _userRepositoryMock.Setup(r => r.GetByEmail(userEmail))
                .ReturnsAsync(user);

            _customerRepositoryMock.Setup(r => r.GetByCpfAndUserId(customerDto.Cpf, user.Id))
                .ReturnsAsync(existingCustomer);

            // Act
            var result = await _customerService.Create(customerDto, userEmail);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.RESOURCE_ALREADY_EXISTS, result.ErrorCode);
            Assert.Equal("Customer already registered", result.ErrorMessage);
        }

        [Fact]
        public async Task Create_ShouldReturnFailure_WhenRepositoryThrowsException()
        {
            // Arrange
            var customerDto = new CustomerRequestDTO
            {
                Name = "John Doe",
                Age = "30",
                CivilState = "Single",
                Cpf = "12345678900",
                City = "São Paulo",
                State = "SP"
            };

            var userEmail = "test@example.com";
            var user = new User { Id = 1, Email = userEmail };

            _userRepositoryMock.Setup(r => r.GetByEmail(userEmail))
                .ReturnsAsync(user);

            _customerRepositoryMock.Setup(r => r.GetByCpfAndUserId(customerDto.Cpf, user.Id))
                .ReturnsAsync((Customer)null);

            _customerRepositoryMock.Setup(r => r.Create(It.IsAny<Customer>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _customerService.Create(customerDto, userEmail);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.DATABASE_ERROR, result.ErrorCode);
            Assert.Equal("An error occurred while creating the customer.", result.ErrorMessage);
        }

        [Fact]
        public async Task GetAll_ShouldReturnSuccess_WithCustomersList()
        {
            // Arrange
            var userId = 1;
            var customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "Customer 1", User = new User { Id = userId } },
                new Customer { Id = 2, Name = "Customer 2", User = new User { Id = userId } }
            };

            _customerRepositoryMock.Setup(r => r.GetAll(userId))
                .ReturnsAsync(customers);

            // Act
            var result = await _customerService.GetAll(userId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);
        }

        [Fact]
        public async Task GetAll_ShouldReturnEmptyList_WhenNoCustomers()
        {
            // Arrange
            var userId = 1;
            var emptyList = new List<Customer>();

            _customerRepositoryMock.Setup(r => r.GetAll(userId))
                .ReturnsAsync(emptyList);

            // Act
            var result = await _customerService.GetAll(userId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task GetAll_ShouldReturnFailure_WhenRepositoryThrowsException()
        {
            // Arrange
            var userId = 1;

            _customerRepositoryMock.Setup(r => r.GetAll(userId))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _customerService.GetAll(userId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.DATABASE_ERROR, result.ErrorCode);
        }

        [Fact]
        public async Task GetForId_ShouldReturnSuccess_WhenCustomerExists()
        {
            // Arrange
            var userId = 1;
            var customerId = 1;
            var customer = new Customer { Id = customerId, Name = "John Doe", User = new User { Id = userId } };

            _customerRepositoryMock.Setup(r => r.GetForId(userId, customerId))
                .ReturnsAsync(customer);

            // Act
            var result = await _customerService.GetForId(userId, customerId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(customerId, result.Data.Id);
        }

        [Fact]
        public async Task GetForId_ShouldReturnFailure_WhenCustomerNotFound()
        {
            // Arrange
            var userId = 1;
            var customerId = 999;

            _customerRepositoryMock.Setup(r => r.GetForId(userId, customerId))
                .ReturnsAsync((Customer)null);

            // Act
            var result = await _customerService.GetForId(userId, customerId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.RESOURCE_NOT_FOUND, result.ErrorCode);
            Assert.Equal("Customer not found.", result.ErrorMessage);
        }

        [Fact]
        public async Task DeleteForId_ShouldReturnSuccess_WhenCustomerExists()
        {
            // Arrange
            var userId = 1;
            var customerId = 1;
            var customer = new Customer { Id = customerId, Name = "John Doe", User = new User { Id = userId } };

            _customerRepositoryMock.Setup(r => r.GetForId(userId, customerId))
                .ReturnsAsync(customer);

            _customerRepositoryMock.Setup(r => r.Delete(customer))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _customerService.DeleteForId(userId, customerId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(customerId, result.Data.Id);
        }

        [Fact]
        public async Task DeleteForId_ShouldReturnFailure_WhenCustomerNotFound()
        {
            // Arrange
            var userId = 1;
            var customerId = 999;

            _customerRepositoryMock.Setup(r => r.GetForId(userId, customerId))
                .ReturnsAsync((Customer)null);

            // Act
            var result = await _customerService.DeleteForId(userId, customerId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.RESOURCE_NOT_FOUND, result.ErrorCode);
        }

        [Fact]
        public async Task Update_ShouldReturnSuccess_WhenCustomerExists()
        {
            // Arrange
            var userId = 1;
            var customerId = 1;
            var existingCustomer = new Customer
            {
                Id = customerId,
                Name = "Old Name",
                Age = "25",
                User = new User { Id = userId }
            };

            var updateDto = new CustomerRequestDTO
            {
                Name = "New Name",
                Age = "30",
                CivilState = "Married",
                Cpf = "12345678900",
                City = "Rio de Janeiro",
                State = "RJ"
            };

            _customerRepositoryMock.Setup(r => r.GetForId(userId, customerId))
                .ReturnsAsync(existingCustomer);

            _customerRepositoryMock.Setup(r => r.Update(It.IsAny<Customer>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _customerService.Update(userId, customerId, updateDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(updateDto.Name, result.Data.Name);
            Assert.Equal(updateDto.Age, result.Data.Age);
            Assert.Equal(updateDto.CivilState, result.Data.CivilState);
        }

        [Fact]
        public async Task Update_ShouldReturnFailure_WhenCustomerNotFound()
        {
            // Arrange
            var userId = 1;
            var customerId = 999;
            var updateDto = new CustomerRequestDTO();

            _customerRepositoryMock.Setup(r => r.GetForId(userId, customerId))
                .ReturnsAsync((Customer)null);

            // Act
            var result = await _customerService.Update(userId, customerId, updateDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.RESOURCE_NOT_FOUND, result.ErrorCode);
        }
    }
}
