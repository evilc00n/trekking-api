
namespace TrekkingApi.Domain.Entity
{
    public class UserSettingsEntity
    {
        public int Id { get; set; }
        public long UserId { get; set; }

        public UserEntity User { get; set; }
    }
}
