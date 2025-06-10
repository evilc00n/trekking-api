
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TrekkingApi.Domain.DTO.User;
using TrekkingApi.Domain.Interfaces.Databases;
using TrekkingApi.Domain.Interfaces.Services;
using TrekkingApi.Domain.Result;

namespace TrekkingApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ISessionService _sessionService;
        private readonly Serilog.ILogger _logger;
        private readonly IUserService _userService;
        public AuthController(IAuthService authService,
            ISessionService sessionService,
            Serilog.ILogger logger,
            IUserService userService)
        {
            _authService = authService;
            _sessionService = sessionService;
            _logger = logger;
            _userService = userService;
        }







        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO dto)
        {
            var responce = await _authService.Register(dto);
            if (responce.IsSuccess)
            {
                return Ok(responce);
            }
            return BadRequest(responce);
        }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.Error("Model validation failed:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.Error($"Validation error: {error.ErrorMessage}");
                }
                return BadRequest(ModelState);
            }

            var responce = await _authService.Login(dto);
            if (!responce.IsSuccess)
            {
                return BadRequest(responce);
            }


            var sessionResult = await _sessionService.CreateSessionAsync(dto.Login);
            if (!sessionResult.IsSuccess)
            {
                return BadRequest(sessionResult);
            }

            var principalResult = _sessionService.GetPrincipals(sessionResult.Data, dto.Login);
            if(!principalResult.IsSuccess)
            {
                return BadRequest(principalResult);
            }

            await HttpContext.SignInAsync("MyCookieAuth", principalResult.Data, new AuthenticationProperties
            {
                IsPersistent = true
            });

            return Ok(sessionResult);
        }



        [Authorize(Policy = "RedisSessionPolicy", AuthenticationSchemes = "MyCookieAuth")]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var sessionId = User.FindFirst("SessionId")?.Value;
            if (sessionId == null) return BadRequest();


            await _sessionService.InvalidateSessionAsync(sessionId);
            await HttpContext.SignOutAsync("MyCookieAuth");

            var result = new BaseResult<string>()
            {
                Data = "Logged Out"
            };


            return Ok(result);
        }



        [Authorize(Policy = "RedisSessionPolicy", AuthenticationSchemes = "MyCookieAuth")]
        [HttpGet("logout-get")]
        public async Task<IActionResult> LogoutGet()
        {
            var sessionId = User.FindFirst("SessionId")?.Value;
            if (sessionId == null) return BadRequest();


            await _sessionService.InvalidateSessionAsync(sessionId);
            await HttpContext.SignOutAsync("MyCookieAuth");

            var result = new BaseResult<string>()
            {
                Data = "Logged Out"
            };


            return Ok(result);
        }


        /// <summary>
        /// Test rout for loginpath
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet("login-path")]
        public async Task<IActionResult> LoginPath()
        {
            return Ok(false);
        }


        /// <summary>
        /// Test rout for access denied path
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet("access-denied-path")]
        public async Task<IActionResult> AccessDeniedPath()
        {
            var result = new BaseResult<string>()
            {
                Data = "Access DENIED"
            };
            return Ok(result);
        }



        /// <summary>
        /// Test rout for checking authorization
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [Authorize(Policy = "RedisSessionPolicy", AuthenticationSchemes = "MyCookieAuth")]
        [HttpGet("check-session")]
        public async Task<IActionResult> CheckSession()
        {
            var result = new BaseResult<string>()
            {
                Data = "You are authorized"
            };
            return Ok(result);
        }





    }


}
