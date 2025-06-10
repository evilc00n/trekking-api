using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TrekkingApi.Application.Resources;
using TrekkingApi.Domain.DTO.Pin;
using TrekkingApi.Domain.DTO.User;
using TrekkingApi.Domain.Entity;
using TrekkingApi.Domain.Enum;
using TrekkingApi.Domain.Interfaces.Repositories;
using TrekkingApi.Domain.Interfaces.Services;
using TrekkingApi.Domain.Result;
using Minio;
using Microsoft.Extensions.Options;
using TrekkingApi.Domain.Options.MinioOptions;

namespace TrekkingApi.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<UserEntity> _userRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IMinioClient _minio;
        private readonly string _bucket;

        public UserService(IBaseRepository<UserEntity> userRepository,
            ILogger logger,
            IMapper mapper,
            IMinioClient minio,
            IOptions<MinioSettings> options)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
            _minio = minio;
            _bucket = options.Value.BucketName;

        }

        public async Task<BaseResult<UserDTO>> GetUserInfoByIdAsync(long id)
        {
            //ДОБАВИТЬ ВАЛИДАЦИИ
            var user = await _userRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return new BaseResult<UserDTO>
                {
                    ErrorMessage = ErrorMessage.UserNotFound,
                    ErrorCode = (int)ErrorCodes.UserNotFound
                };
            }

            return new BaseResult<UserDTO>()
            {
                Data = _mapper.Map<UserDTO>(user)
            };
        }

        public async Task<BaseResult<UserDTO>> GetUserInfoByNameAsync(string userName)
        {
            //ДОБАВИТЬ ВАЛИДАЦИИ
            var user = await _userRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Login == userName);

            if (user == null)
            {
                return new BaseResult<UserDTO>
                {
                    ErrorMessage = ErrorMessage.UserNotFound,
                    ErrorCode = (int)ErrorCodes.UserNotFound
                };
            }

            return new BaseResult<UserDTO>()
            {
                Data = _mapper.Map<UserDTO>(user)
            };
        }

        public async Task<CollectionResult<GetPinDTO>> GetUsersPinsAsync(string username)
        {
            try
            {
                // Получаем пользователя по логину
                var user = await _userRepository.GetAll()
                    .Where(u => u.Login == username)
                    .Include(u => u.Pins)
                        .ThenInclude(p => p.Meta)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return new CollectionResult<GetPinDTO>
                    {
                        ErrorMessage = ErrorMessage.UserNotFound,
                        ErrorCode = (int)ErrorCodes.UserNotFound
                    };
                }

                var pins = user.Pins
                    .Where(p => p.Meta != null && !string.IsNullOrEmpty(p.FullUrl))
                    .ToList();

                var resultList = new List<GetPinDTO>();

                foreach (var pin in pins)
                {

                    // Маппим в DTO
                    var dto = new GetPinDTO
                    {
                        Meta = new GetPinMetaDTO
                        {
                            Title = pin.Title,
                            Description = pin.Description,
                            CollectionId = "UNKNOWN",         // Можно достать из pin.CollectionsPins
                            Link = pin.FullUrl,
                            Tags = Array.Empty<string>(),     // Добавить позже
                            Location = "NOT_IMPLEMENTED",     // Аналогично
                            FileMeta = new FileInfoDTO
                            {
                                Width = pin.Meta.Width,
                                Height = pin.Meta.Height,

                            }
                        }
                    };

                    resultList.Add(dto);
                }

                return new CollectionResult<GetPinDTO> 
                { 
                    Data = resultList.ToArray(), 
                    Count = resultList.Count
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while retrieving pins for user {Username}", username);
                return new CollectionResult<GetPinDTO>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError
                };
            }
        }




    }

}
