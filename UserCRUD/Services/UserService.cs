using System.Data;
using UserCRUD.Common;
using UserCRUD.DTOs.Request;
using UserCRUD.DTOs.Response;
using UserCRUD.Models;
using UserCRUD.Repository.Interface;
using UserCRUD.Services.Interfaces;

namespace UserCRUD.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncryptionPasswordService _encryptionPasswordService;
        public UserService(IUserRepository userRepository,
            IEncryptionPasswordService encryptionPasswordService)
        {
            _userRepository = userRepository;
            _encryptionPasswordService = encryptionPasswordService;
        }
        public async Task<Result<User>> Create(UserRequestDTO userDTO)
        {
            var user = await _userRepository.GetByEmail(userDTO.Email);
            if (user != null)
                return Result<User>.Failure("Username or password is incorrect", ErrorCode.USER_ALREADY_EXISTS);

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
            catch (Exception ex)
            {
                // CRIAR SERVIÇO DE LOG
                return Result<User>.Failure("An error occurred while creating the user.", ErrorCode.DATABASE_ERROR);
            }
        }

        public async Task<Result<string>> Login(LoginRequestDTO loginDTO)
        {
            var user = await _userRepository.GetByEmail(loginDTO.Email);
            var loginValid = _encryptionPasswordService.ValidatePassword(loginDTO.Password, user.Password);

            if (!loginValid)
                return Result<string>.Failure("Invalid email or password.", ErrorCode.INVALID_CREDENTIALS);

            // gerar o token JWT
            // var jwt = _jwtService.GenerateToken(user);
            string jwt = "fake-jwt";

            return Result<string>.Success(jwt);

        }
    }
}
