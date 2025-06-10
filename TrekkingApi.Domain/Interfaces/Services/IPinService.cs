
using TrekkingApi.Domain.DTO.Pin;
using TrekkingApi.Domain.DTO.User;
using TrekkingApi.Domain.Result;

namespace TrekkingApi.Domain.Interfaces.Services
{
    public interface IPinService
    {
        Task<BaseResult<long>> CreatePin(PinRequestDTO pinRequest, string username);
        Task<BaseResult<GetPinDTO>> GetPinByIdAsync(long pinId);
    }
}
