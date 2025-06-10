
using TrekkingApi.Domain.DTO.User;
using TrekkingApi.Domain.Result;

namespace TrekkingApi.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<UserDTO>> Register(RegisterUserDTO dto);

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult> Login(LoginUserDTO dto);
    }
}
