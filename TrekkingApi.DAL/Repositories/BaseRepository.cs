﻿
using Microsoft.EntityFrameworkCore;
using TrekkingApi.Domain.Interfaces.Repositories;

namespace TrekkingApi.DAL.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly DbUsersContext _dbContext;

        public BaseRepository(DbUsersContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>();
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Entity is null");

            await _dbContext.AddAsync(entity);
            return entity;
        }

        public TEntity Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Entity is null");

            _dbContext.Update(entity);

            return entity;
        }


        public void Remove(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Entity is null");

            _dbContext.Remove(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
