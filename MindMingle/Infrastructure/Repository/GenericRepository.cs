using System;
using System.Linq;
using Application.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly MMDbContext mMDbContext;
        private readonly DbSet<T> db;

        public GenericRepository(MMDbContext mMDbContext, DbSet<T> db)
        {
            this.mMDbContext = mMDbContext;
            this.db = db;
        }

        public async Task AddAsync(T entity)
        {
            if (entity != null)
                await db.AddAsync(entity);
            else
                throw new Exception();
        }

       public async Task AddRangeAsync(List<T> entities)
        {
            if (entities.Count > 0)
                await db.AddRangeAsync(entities);
            else
                throw new Exception(); 
        }

        public async Task<int> CountAsync()
        {
            return await db.CountAsync();
        }

        public async Task<List<T>> GetAllAsync(System.Linq.Expressions.Expression<Func<T, bool>>? filter)
        {
            List<T> entity;
            if (filter != null)
            entity = await db.Where(filter).ToListAsync();
            else
            entity = await db.ToListAsync();
            return entity;
        }

        public async Task<List<T>> GetAllAsync(System.Linq.Expressions.Expression<Func<T, bool>>? filter, Func<IQueryable<T>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<T, object>>? include, int pageIndex = 1, int pageSize = 10) // Pagination 1-10
        {
            List<T> entity;
            if (filter != null)
                entity = await db.Where(filter).ToListAsync();
            else
                entity = await db.ToListAsync();
            return entity;
        }

        public async Task<T> GetAsync(System.Linq.Expressions.Expression<Func<T, bool>> filter)
        {
            var entity = await db.FirstOrDefaultAsync(filter);
            return entity!;
        }

        Task<T> IGenericRepository<T>.GetAsync(System.Linq.Expressions.Expression<Func<T, bool>> filter, Func<IQueryable<T>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<T, object>>? include)
        {
            throw new NotImplementedException();
        }

        public async Task<T> RemoveByIdAsync(object id)
        {
                var entity = await db.FindAsync(id);
                if (entity != null)
                {
                    db.Remove(entity);
                    await mMDbContext.SaveChangesAsync();
                    return entity;
                }
                else
                {
                    throw new Exception();
                }
            
            

        }
    }
}

