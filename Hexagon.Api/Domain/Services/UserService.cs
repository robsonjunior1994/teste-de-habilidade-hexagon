using Hexagon.Api.Domain.Models;
using Hexagon.Api.Domain.Services.Interfaces;
using Hexagon.Api.Infrastruture.Repository.Interface;
using Hexagon.Api.Presentation.Common;
using Hexagon.Api.Presentation.DTOs.Request;

namespace Hexagon.Api.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncryptionPasswordService _encryptionPasswordService;
        private readonly IJwtService _jwtService;
        public UserService(IUserRepository userRepository,
            IEncryptionPasswordService encryptionPasswordService,
            IJwtService jwtService)
        {
            _userRepository = userRepository;
            _encryptionPasswordService = encryptionPasswordService;
            _jwtService = jwtService;
        }
        public async Task<Result<User>> Create(UserRequestDTO userDTO)
        {
            var user = await _userRepository.GetByEmail(userDTO.Email);
            if (user != null)
                return Result<User>.Failure("Username or password is incorrect", ErrorCode.RESOURCE_ALREADY_EXISTS);

            var newUser = new User
            {

                Name = userDTO.Name,
                Email = userDTO.Email,
                Password = _encryptionPasswordService.EncryptPassword(userDTO.Password),
            };

            try
            {
                await _userRepository.Create(newUser);
                return Result<User>.Success(newUser);
            }
            catch
            {
                // CRIAR SERVIÇO DE LOG
                return Result<User>.Failure("An error occurred while creating the user.", ErrorCode.DATABASE_ERROR);
            }
        }

        public async Task<Result<string>> Login(LoginRequestDTO loginDTO)
        {
            var user = await _userRepository.GetByEmail(loginDTO.Email);
            if(user == null)
                return Result<string>.Failure("User not found", ErrorCode.RESOURCE_NOT_FOUND);

            var loginValid = _encryptionPasswordService.ValidatePassword(loginDTO.Password, user?.Password);

            if (!loginValid)
                return Result<string>.Failure("Invalid email or password.", ErrorCode.INVALID_CREDENTIALS);

            string jwt = _jwtService.GenerateToken(user);

            return Result<string>.Success(jwt);

        }
    }
}
