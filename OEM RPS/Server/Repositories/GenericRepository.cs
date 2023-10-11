using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OEM_RPS.Shared.DTO;
using OEM_RPS.Shared.Model;
using OEMRPS.Server.DataBaseContext;

namespace OEMRPS.Server.Repositories
{
    public interface IGenericRepository<T> where T: BaseEntity
    {
        IQueryable<T> Queryable();

        Task<T?> GetById(int id);
        Task<IEnumerable<T>> GetAll();
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate);

        Task<DbResultDTO<T>> Add(T entity);
        Task<DbResultDTO<T>> Update(T entity);
        Task<DbResultDTO<T>> Delete(T entity);
        Task<DbResultDTO<T>> DeleteRange(List<T> entities);
        Task SaveAsync();
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        //private readonly DbContext dbContext;
        private readonly Context dbContext;
        private readonly DbSet<T> dbSet;

        public GenericRepository(Context _dbContext)
        {
            dbContext = _dbContext ?? throw new ArgumentException(nameof(T));
            dbSet = _dbContext.Set<T>();
        }

        private IQueryable<T> All => dbSet.Cast<T>();
        public IQueryable<T> Queryable() => dbSet.AsQueryable();


        public virtual async Task<DbResultDTO<T>> Add(T entity)
        {
            try
            {
                if (entity != null)
                {
                    dbSet.Add(entity);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                return new DbResultDTO<T>
                {
                    IsSuccess = false,
                    ErrorMessage = $"Failed to Add: {typeof(T).Name}, \n Ex: {ex.Message}"
                };
            }

            return new DbResultDTO<T>
            {
                IsSuccess = true,
                Entity = entity
            };

        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await dbSet.ToListAsync();
        }

        public virtual async Task<T?> GetById(int Id)
        {
            return await dbSet.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public virtual async Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<DbResultDTO<T>> Update(T entity)
        {
            try
            {
                if(entity != null)
                {
                    dbSet.Update(entity);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                return new DbResultDTO<T>
                {
                    IsSuccess = false,
                    ErrorMessage = $"Failed to Update: {typeof(T).Name}, \n Ex: {ex.Message}"
                };
            }

            return new DbResultDTO<T>
            {
                IsSuccess = true
            };
        }

        public virtual async Task<DbResultDTO<T>> Delete(T entity)
        {
            try
            {
                if (entity != null)
                {
                    dbSet.Remove(entity);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                return new DbResultDTO<T>
                {
                    IsSuccess = false,
                    ErrorMessage = $"Failed to Delete: {typeof(T).Name}, \n Ex: {ex.Message}"
                };
            }

            return new DbResultDTO<T>
            {
                IsSuccess = true
            };
        }

        public virtual async Task<DbResultDTO<T>> DeleteRange(List<T> entities)
        {
            try
            {
                if (entities.Count > 0)
                {
                    dbSet.RemoveRange(entities);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                return new DbResultDTO<T>
                {
                    IsSuccess = false,
                    ErrorMessage = $"Failed to bulk Delete: {typeof(T).Name}, \n Ex: {ex.Message}"
                };
            }

            return new DbResultDTO<T>
            {
                IsSuccess = true
            };
        }

        public Task SaveAsync() => dbContext.SaveChangesAsync();
    }
}

