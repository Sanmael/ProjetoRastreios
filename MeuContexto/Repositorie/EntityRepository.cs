using Entities.Interfaces;
using MeuContexto.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;

namespace MeuContexto.Repositorie
{
    public class EntityRepository : IRepository
    {
        private readonly AppDbContext _appDbContext;

        public EntityRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<T>> GetEntityByProcedure<T>(string proc, KeyValuePair<string, object>? parameters = null) where T : class
        {
            return await _appDbContext.Set<T>().FromSqlRaw(proc).ToListAsync();
        }

        public async Task<T> GetEntityAsync<T>(Func<T, bool> predicate) where T : class
        {
            return _appDbContext.Set<T>().AsNoTracking().FirstOrDefault(predicate);
        }

        public async Task<List<T>> GetEntitys<T>(Func<T, bool>? predicate = null) where T : class
        {
            if (predicate == null)
            {
                return await _appDbContext.Set<T>().AsNoTracking().ToListAsync();
            }
            return (List<T>)_appDbContext.Set<T>().AsNoTracking().ToList().Where(predicate);
        }

        public async Task RemoveEntityAsync<T>(T entity) where T : class
        {
            _appDbContext.Set<T>().Remove(entity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task SaveEntityAsync<T>(T entity) where T : class
        {
            await _appDbContext.Set<T>().AddAsync(entity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateEntityAsync<T>(T entity) where T : class
        {
            _appDbContext.Set<T>().Update(entity);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
