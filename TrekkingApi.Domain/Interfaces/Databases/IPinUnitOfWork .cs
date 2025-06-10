
using TrekkingApi.Domain.Entity;
using TrekkingApi.Domain.Interfaces.Repositories;

namespace TrekkingApi.Domain.Interfaces.Databases
{
    public interface IPinUnitOfWork : IUnitOfWorkBase
    {
        IBaseRepository<PinEntity> Pins { get; set; }
        IBaseRepository<UserEntity> Users { get; set; }
        IBaseRepository<PinMetaEntity> PinsMeta { get; set; }
    }
}
