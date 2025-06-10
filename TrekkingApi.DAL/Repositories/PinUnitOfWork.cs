
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Net;
using TrekkingApi.Domain.Entity;
using TrekkingApi.Domain.Interfaces.Databases;
using TrekkingApi.Domain.Interfaces.Repositories;

namespace TrekkingApi.DAL.Repositories
{
    public class PinUnitOfWork : IPinUnitOfWork
    {
        private readonly DbUsersContext _context;
        public IBaseRepository<PinEntity> Pins { get; set; }
        public IBaseRepository<PinMetaEntity> PinsMeta { get; set; }
        public IBaseRepository<UserEntity> Users { get; set; }


        public PinUnitOfWork(DbUsersContext context,
                IBaseRepository<PinEntity> pins,
                IBaseRepository<PinMetaEntity> pinsMeta,
                IBaseRepository<UserEntity> users)
        {
            _context = context;
            Pins = pins;
            PinsMeta = pinsMeta;
            Users = users;
        }





        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }


        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
