
namespace TrekkingApi.Domain.DTO.User
{
    public record UserDTO(long Id, string Login, string? Name, string? AvatarUrl, string? Description);
}
