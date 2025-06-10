
namespace TrekkingApi.Domain.DTO.User
{
    public record RegisterUserDTO(string Login, string Password, string? Name, string? AvatarUrl, string? Description);
}
