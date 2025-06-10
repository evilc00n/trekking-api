
namespace TrekkingApi.Domain.Entity
{
    public class CollectionEntity
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }

        public UserEntity Owner { get; set; }
        public ICollection<CollectionsPinsEntity> CollectionsPins { get; set; }
    }
}
