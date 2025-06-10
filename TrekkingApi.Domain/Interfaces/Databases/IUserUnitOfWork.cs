
using TrekkingApi.Domain.Entity;
using TrekkingApi.Domain.Interfaces.Repositories;

namespace TrekkingApi.Domain.Interfaces.Databases
{
    public interface IUserUnitOfWork : IUnitOfWorkBase
    {
        IBaseRepository<UserEntity> Users { get; set; }
        IBaseRepository<CredentialEntity> Credentials { get; set; }
    }
}
