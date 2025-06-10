
namespace TrekkingApi.Domain.Entity
{
    public class PinMetaEntity
    {
        public long PinId { get; set; }
        public int Size { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int? LocationId { get; set; }

        public PinEntity Pin { get; set; }
        public LocationEntity Location { get; set; }
    }
}
