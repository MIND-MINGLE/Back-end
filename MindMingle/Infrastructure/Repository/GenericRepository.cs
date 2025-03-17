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

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = db;
            if (include != null)
            {
                query = include(query);
            }
            return await query.FirstOrDefaultAsync(filter);
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

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null");
            }

            try
            {
                db.Update(entity); // Đánh dấu thực thể là Modified
                await mMDbContext.SaveChangesAsync(); // Lưu các thay đổi vào cơ sở dữ liệu
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
                throw; // Ném lại ngoại lệ để xử lý ở tầng trên
            }
        }

        public async Task UpdateFieldAsync<TKey>(TKey id, Expression<Func<T, object>> propertyExpression, object newValue)
        {
            if (id == null || propertyExpression == null || newValue == null)
            { 
                throw new ArgumentNullException("ID, property expression, or new value cannot be null");
            }

            try
            {
                // Tìm entity theo ID
                var entity = await db.FindAsync(id);
                if (entity == null)
                {
                    throw new Exception($"Entity with ID {id} not found");
                }

                // Lấy tên thuộc tính từ biểu thức lambda
                var propertyName = GetPropertyName(propertyExpression);

                // Cập nhật giá trị cho field được chỉ định
                var propertyInfo = typeof(T).GetProperty(propertyName);
                if (propertyInfo == null)
                {
                    throw new Exception($"Property {propertyName} not found on type {typeof(T).Name}");
                }

                // Đảm bảo giá trị mới phù hợp với kiểu của thuộc tính
                var convertedValue = Convert.ChangeType(newValue, propertyInfo.PropertyType);
                propertyInfo.SetValue(entity, convertedValue);

                // Đánh dấu chỉ field này là Modified
                mMDbContext.Entry(entity).Property(propertyName).IsModified = true;

                // Lưu thay đổi vào cơ sở dữ liệu
                await mMDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateFieldAsync: {ex.Message}");
                throw;
            }
        }

        private string GetPropertyName(Expression<Func<T, object>> propertyExpression)
        {
            if (propertyExpression.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }
            else if (propertyExpression.Body is UnaryExpression unaryExpression && unaryExpression.Operand is MemberExpression unaryMemberExpression)
            {
                return unaryMemberExpression.Member.Name;
            }
            throw new ArgumentException("Invalid property expression", nameof(propertyExpression));
        }

        public async Task UpdateFieldsAsync<TKey>(TKey id, Dictionary<string, object> fieldsToUpdate)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "ID cannot be null");
            }

            if (fieldsToUpdate == null || !fieldsToUpdate.Any())
            {
                throw new ArgumentNullException(nameof(fieldsToUpdate), "Fields to update cannot be null or empty");
            }

            try
            {
                // Find the entity by ID
                var entity = await db.FindAsync(id);
                if (entity == null)
                {
                    throw new Exception($"Entity with ID {id} not found");
                }

                // Get the entry for tracking changes
                var entry = mMDbContext.Entry(entity);

                // Update each field
                foreach (var field in fieldsToUpdate)
                {
                    var propertyName = field.Key;
                    var newValue = field.Value;

                    // Verify the property exists
                    var propertyInfo = typeof(T).GetProperty(propertyName);
                    if (propertyInfo == null)
                    {
                        throw new Exception($"Property {propertyName} not found on type {typeof(T).Name}");
                    }

                    // Convert the value to the correct type
                    try
                    {
                        var convertedValue = newValue == null
                            ? null
                            : Convert.ChangeType(newValue, propertyInfo.PropertyType);
                        propertyInfo.SetValue(entity, convertedValue);

                        // Mark the property as modified
                        entry.Property(propertyName).IsModified = true;
                    }
                    catch (FormatException ex)
                    {
                        throw new Exception($"Failed to convert value for property {propertyName}: {ex.Message}");
                    }
                    catch (InvalidCastException ex)
                    {
                        throw new Exception($"Invalid cast for property {propertyName}: {ex.Message}");
                    }
                }

                // Save changes to database
                await mMDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateFieldsAsync: {ex.Message}");
                throw;
            }
        }
    }
}

