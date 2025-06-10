
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using TrekkingApi.Domain.Entity;
using TrekkingApi.Domain.Interfaces.Databases;
using TrekkingApi.Domain.Interfaces.Repositories;

namespace TrekkingApi.DAL.Repositories
{
    public class UserUnitOfWork : IUserUnitOfWork
    {
        private readonly DbUsersContext _context;

        public UserUnitOfWork(DbUsersContext context,
            IBaseRepository<UserEntity> users, 
            IBaseRepository<CredentialEntity> credentials)
        {
            _context = context;
            Users = users;
            Credentials = credentials;
        }

        public IBaseRepository<UserEntity> Users { get; set; }
        public IBaseRepository<CredentialEntity> Credentials { get; set; }

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
