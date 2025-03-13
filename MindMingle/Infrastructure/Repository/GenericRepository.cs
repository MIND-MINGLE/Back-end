using System;
using System.Linq;
using System.Linq.Expressions;
using Application.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly MMDbContext mMDbContext;
        private readonly DbSet<T> db;

        public GenericRepository(MMDbContext mMDbContext)
        {
            this.mMDbContext = mMDbContext;
            db = mMDbContext.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            if (entity != null)
            {
                await db.AddAsync(entity);
                await mMDbContext.SaveChangesAsync();
            }
            else
                throw new Exception();
        }

       public async Task AddRangeAsync(List<T> entities)
        {
            if (entities.Count > 0)
                await mMDbContext.Set<T>().AddRangeAsync(entities);
            else
                throw new Exception(); 
        }

        public async Task<int> CountAsync()
        {
            return await mMDbContext.Set<T>().CountAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter)
        {
            try
            {
                if (filter != null)
                {
                    Console.WriteLine("Getting Data with filter...");
                    return await db.Where(filter).ToListAsync();
                }
                else
                {
                    Console.WriteLine("Getting Data...");
                   
                    return await db.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllAsync: {ex.Message}");
                return new List<T>();
            }
        }


        public async Task<List<T>> GetAllAsync(
                Expression<Func<T, bool>>? filter = null,
                Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
                int pageIndex = 1, int pageSize = 10)
        {
            IQueryable<T> query = db;

            if (filter != null)
                query = query.Where(filter);

            if (include != null)
                query = include(query); // ✅ Apply Include()
                                        // Áp dụng phân trang
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return await query.ToListAsync(); // ✅ Ensure execution
            // I do not know how to fix this. thank you chat
        }


        public async Task<T> GetAsync(System.Linq.Expressions.Expression<Func<T, bool>> filter)
        {
            return await db.FirstOrDefaultAsync(filter);
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

