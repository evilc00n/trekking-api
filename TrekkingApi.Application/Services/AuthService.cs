using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Cryptography;
using System.Text;
using TrekkingApi.Application.Resources;
using TrekkingApi.Domain.DTO.User;
using TrekkingApi.Domain.Entity;
using TrekkingApi.Domain.Enum;
using TrekkingApi.Domain.Interfaces.Databases;
using TrekkingApi.Domain.Interfaces.Repositories;
using TrekkingApi.Domain.Interfaces.Services;
using TrekkingApi.Domain.Result;

namespace TrekkingApi.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseRepository<UserEntity> _userRepository;
        private readonly IBaseRepository<CredentialEntity> _credentialsRepository;
        private readonly IUserUnitOfWork _userUnitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public AuthService(IBaseRepository<UserEntity> userRepository,
            ILogger logger, IMapper mapper, IUserUnitOfWork userUnitOfWork, 
            IBaseRepository<CredentialEntity> credentialsRepository)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
            _userUnitOfWork = userUnitOfWork;
            _credentialsRepository = credentialsRepository;
        }







        public async Task<BaseResult> Login(LoginUserDTO dto)
        {
            var user = await _userRepository.GetAll()
                .Include(u => u.Credential)
                .FirstOrDefaultAsync(x => x.Login == dto.Login);

            if (user == null || user.Credential == null)
            {
                return new BaseResult
                {
                    ErrorMessage = ErrorMessage.UserNotFound,
                    ErrorCode = (int)ErrorCodes.UserNotFound
                };
            }

            if (!IsVerifyPassword(user.Credential.Hash, dto.Password))
            {
                return new BaseResult
                {
                    ErrorMessage = ErrorMessage.PasswordIsWrong,
                    ErrorCode = (int)ErrorCodes.PasswordIsWrong
                };
            }

            return new BaseResult();
        }




        /// <inheritdoc />
        public async Task<BaseResult<UserDTO>> Register(RegisterUserDTO dto)
        {
            var user = await _userRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Login == dto.Login);
            if (user != null)
            {
                return new BaseResult<UserDTO>()
                {
                    ErrorMessage = ErrorMessage.UserAlreadyExists,
                    ErrorCode = (int)ErrorCodes.UserAlreadyExists
                };
            }
            var hashUserPassword = HashPassword(dto.Password);

            using (var transaction = await _userUnitOfWork.BeginTransactionAsync())
            {
                try
                {
                    user = new UserEntity()
                    {
                        Login = dto.Login,
                        Name = dto.Name,
                        AvatarUrl = dto.AvatarUrl,
                        Description = dto.Description
                    };

                    await _userUnitOfWork.Users.CreateAsync(user);
                    await _userUnitOfWork.SaveChangesAsync();

                    var credentials = new CredentialEntity()
                    {
                        UserId = user.Id,
                        Hash = HashPassword(dto.Password)
                    };

                    await _userUnitOfWork.Credentials.CreateAsync(credentials);
                    await _userUnitOfWork.SaveChangesAsync();

                    await transaction.CommitAsync();

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.Error(ex.ToString());
                    return new BaseResult<UserDTO>()
                    {
                        ErrorMessage = ErrorMessage.InternalServerError,
                        ErrorCode = (int)ErrorCodes.InternalServerError
                    };
                }
            }


            return new BaseResult<UserDTO>()
            {
                Data = _mapper.Map<UserDTO>(user)
            };

        }

        private string HashPassword(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private bool IsVerifyPassword(string userPasswordHash, string userPassword)
        {
            var hash = HashPassword(userPassword);
            return hash == userPasswordHash;
        }
    }
}
