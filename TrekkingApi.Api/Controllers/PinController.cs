
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TrekkingApi.Application.Services;
using TrekkingApi.Domain.DTO.Pin;
using TrekkingApi.Domain.Interfaces.Services;

namespace TrekkingApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PinController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly IPinService _pinService;


        public PinController(Serilog.ILogger logger, 
            IPinService pinService)
        {
            _logger = logger;
            _pinService = pinService;
        }

        /// <summary>
        /// Создание пина
        /// </summary>
        /// <param name="pinRequest"></param>
        /// <returns></returns>
        [Authorize(Policy = "RedisSessionPolicy", AuthenticationSchemes = "MyCookieAuth")]
        [HttpPost("create-pin")]
        [Consumes("application/cbor")]
        public async Task<IActionResult> CreatePin([FromBody] PinRequestDTO pinRequest)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            //var username = "test1489";
            var responce = await _pinService.CreatePin(pinRequest, username);
            if (responce.IsSuccess)
            {
                return Ok(responce);
            }
            return BadRequest(responce);

        }

        /// <summary>
        /// Получение пина по его id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get-pin/{id}")]
        public async Task<IActionResult> GetPinById(long id)
        {


            var result = await _pinService.GetPinByIdAsync(id);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }


            return Ok(result);
        }




    }
}
