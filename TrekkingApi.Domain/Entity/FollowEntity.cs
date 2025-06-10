

namespace TrekkingApi.Domain.Entity
{
    public class FollowEntity
    {
        public long FollowerId { get; set; }
        public long FollowingId { get; set; }
        public DateTime CreatedAt { get; set; }

        public UserEntity Follower { get; set; }
        public UserEntity Following { get; set; }
    }
}
