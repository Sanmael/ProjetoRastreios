using ProjetoJqueryEstudos.Entities;

namespace ProjetoJqueryEstudos.Interfaces
{
    public interface IRepository
    {
        public void SaveEntity<T>(T entity) where T : class;
        public void UpdateEntity<T>(T entity) where T : class;
        public void RemoveEntity<T>(T entity) where T : class;
        public T GetEntity<T>(Func<T, bool> predicate) where T : class;
        public IEnumerable<T> GetEntitys<T>(Func<T, bool>? predicate = null) where T : class;
        public List<T> GetEntityByProcedure<T>(string proc) where T : class;
    }
}
