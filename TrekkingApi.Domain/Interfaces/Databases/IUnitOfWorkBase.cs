
using Microsoft.EntityFrameworkCore.Storage;
using TrekkingApi.Domain.Entity;
using TrekkingApi.Domain.Interfaces.Repositories;

namespace TrekkingApi.Domain.Interfaces.Databases
{
    public interface IUnitOfWorkBase : IStateSaveChanges
    {
        Task<IDbContextTransaction> BeginTransactionAsync();



    }
}
