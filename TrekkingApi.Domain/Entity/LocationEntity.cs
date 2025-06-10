

namespace TrekkingApi.Domain.Entity
{
    public class LocationEntity
    {
        public int Id { get; set; }
        public int Lat { get; set; }
        public int Lng { get; set; }
        public string AddressName { get; set; }

        public ICollection<PinMetaEntity> PinMetas { get; set; }
    }
}
