using Entities;

namespace Entities.Interfaces
{
    public interface IRepository
    {
        public Task SaveEntityAsync<T>(T entity) where T : class;
        public Task UpdateEntityAsync<T>(T entity) where T : class;
        public Task RemoveEntityAsync<T>(T entity) where T : class;
        public Task<T> GetEntityAsync<T>(Func<T, bool> predicate) where T : class;
        public Task<List<T>> GetEntitys<T>(Func<T, bool>? predicate = null) where T : class;
        public Task<List<T>> GetEntityByProcedure<T>(string proc, KeyValuePair<string,object>? parameters = null) where T : class;
    }
}
