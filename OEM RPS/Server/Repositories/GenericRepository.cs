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

        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAll();
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate);

        Task<DbResultDTO> Add(T entity);
        Task<DbResultDTO> Update(T entity);
        Task<DbResultDTO> Delete(T entity);
        Task<DbResultDTO> DeleteRange(List<T> entities);
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


        public async Task<DbResultDTO> Add(T entity)
        {
            try
            {
                dbSet.Add(entity);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new DbResultDTO
                {
                    IsSuccess = false,
                    ErrorMessage = $"Failed to Add: {typeof(T).Name}, \n Ex: {ex.Message}"
                };
            }

            return new DbResultDTO
            {
                IsSuccess = true
            };

        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<T> GetById(int Id)
        {
            return await dbSet.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.Where(predicate).ToListAsync();
        }

        public async Task<DbResultDTO> Update(T entity)
        {
            try
            {
                dbSet.Update(entity);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new DbResultDTO
                {
                    IsSuccess = false,
                    ErrorMessage = $"Failed to Update: {typeof(T).Name}, \n Ex: {ex.Message}"
                };
            }

            return new DbResultDTO
            {
                IsSuccess = true
            };
        }

        public async Task<DbResultDTO> Delete(T entity)
        {
            try
            {
                dbSet.Remove(entity);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new DbResultDTO
                {
                    IsSuccess = false,
                    ErrorMessage = $"Failed to Delete: {typeof(T).Name}, \n Ex: {ex.Message}"
                };
            }

            return new DbResultDTO
            {
                IsSuccess = true
            };
        }

        public async Task<DbResultDTO> DeleteRange(List<T> entities)
        {
            try
            {
                dbSet.RemoveRange(entities);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new DbResultDTO
                {
                    IsSuccess = false,
                    ErrorMessage = $"Failed to bulk Delete: {typeof(T).Name}, \n Ex: {ex.Message}"
                };
            }

            return new DbResultDTO
            {
                IsSuccess = true
            };
        }

        public Task SaveAsync() => dbContext.SaveChangesAsync();
    }
}

