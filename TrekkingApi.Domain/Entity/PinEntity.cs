

namespace TrekkingApi.Domain.Entity
{
    public class PinEntity
    {
        public long Id { get; set; }
        public long? OwnerId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Category { get; set; }
        public string FullUrl { get; set; }

        public UserEntity Owner { get; set; }
        public PinMetaEntity Meta { get; set; }
        public ICollection<CollectionsPinsEntity> CollectionsPins { get; set; }
        public ICollection<PinViewEntity> Views { get; set; }
    }
}
