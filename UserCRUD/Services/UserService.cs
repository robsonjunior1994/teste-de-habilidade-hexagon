using System.Data;
using UserCRUD.Common;
using UserCRUD.DTOs;
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
        public async Task<Result<User>> Create(UserDTO userDTO)
        {
            var user = await _userRepository.GetByEmail(userDTO.Email);
            if (user != null)
                return Result<User>.Failure("User already exist", ErrorCode.USER_ALREADY_EXISTS);

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
    }
}
