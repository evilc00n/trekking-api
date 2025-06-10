
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minio.DataModel.Args;
using Minio.DataModel;
using System.Net.NetworkInformation;
using System.Security.Claims;
using TrekkingApi.Domain.Interfaces.Databases;
using TrekkingApi.Domain.Interfaces.Services;
using TrekkingApi.Domain.Result;
using TrekkingApi.Application.Services;

namespace TrekkingApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly Serilog.ILogger _logger;
        private readonly IUserService _userService;

        public UserController(
            Serilog.ILogger logger,
            IUserService userService,
            IPinService pinService)
        {

            _logger = logger;
            _userService = userService;
        }







        /// <summary>
        /// Получение информации об авторизованном пользователе
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "RedisSessionPolicy", AuthenticationSchemes = "MyCookieAuth")]
        [HttpGet("get-me")]
        public async Task<IActionResult> GetAuthorizedUserInfo()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("User authorized, but name is empty");
            }


            var result = await _userService.GetUserInfoByNameAsync(username);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }


            return Ok(result);
        }



        /// <summary>
        /// Получение информации о пользователе по его login
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("get-user/{name}")]
        public async Task<IActionResult> GetUserInfoByName(string name)
        {


            var result = await _userService.GetUserInfoByNameAsync(name);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }


            return Ok(result);
        }



        /// <summary>
        /// Получение пинов авторизованного пользователя
        /// </summary>
        /// <returns></returns>
        [HttpGet("{username}/get-pins")]
        public async Task<IActionResult> GetAuthorizedUserPins(string username)
        {

            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("User authorized, but name is empty");
            }


            var result = await _userService.GetUsersPinsAsync(username);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }


            return Ok(result);
        }
    }
}
