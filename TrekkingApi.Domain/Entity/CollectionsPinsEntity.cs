
using System.Net.NetworkInformation;

namespace TrekkingApi.Domain.Entity
{
    public class CollectionsPinsEntity
    {
        public long PinId { get; set; }
        public long CollectionId { get; set; }

        public PinEntity Pin { get; set; }
        public CollectionEntity Collection { get; set; }
    }
}
