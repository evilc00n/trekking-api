

namespace TrekkingApi.Domain.Entity
{
    public class PinViewEntity
    {
        public int Id { get; set; }
        public long PinId { get; set; }
        public long UserId { get; set; }

        public PinEntity Pin { get; set; }
        public UserEntity User { get; set; }
    }
}
