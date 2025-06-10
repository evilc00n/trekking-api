
using TrekkingApi.Domain.Enum.DatabasesEnums;

namespace TrekkingApi.Domain.Entity
{
    public class LikeEntity
    {
        public long EntityId { get; set; }
        public EntityType Type { get; set; }
    }
}
