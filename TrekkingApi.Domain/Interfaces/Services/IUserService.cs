
using TrekkingApi.Domain.DTO.Pin;
using TrekkingApi.Domain.DTO.User;
using TrekkingApi.Domain.Result;

namespace TrekkingApi.Domain.Interfaces.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Получение информации о пользователе по его username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<BaseResult<UserDTO>> GetUserInfoByNameAsync(string userName);


        /// <summary>
        /// Получение информации о пользователе по его id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResult<UserDTO>> GetUserInfoByIdAsync(long id);


        /// <summary>
        /// Получение пинов пользователя
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<CollectionResult<GetPinDTO>> GetUsersPinsAsync(string username);
    }
}
