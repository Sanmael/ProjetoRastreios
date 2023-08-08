using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProjetoJqueryEstudos.Context;
using ProjetoJqueryEstudos.Entities;
using ProjetoJqueryEstudos.Interfaces;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;

namespace ProjetoJqueryEstudos.Repositorie
{
    public class Repository : IRepository
    {
        private readonly AppDbContext _appDbContext;

        public Repository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public List<T> GetEntityByProcedure<T>(string proc) where T : class
        {
            return _appDbContext.Set<T>().FromSqlRaw(proc).ToList();
        }

        public T GetEntity<T>(Func<T, bool> predicate) where T : class
        {
            return _appDbContext.Set<T>().AsNoTracking().FirstOrDefault(predicate);
        }

        public IEnumerable<T> GetEntitys<T>(Func<T, bool>? predicate = null) where T : class
        {
            if (predicate == null)
            {
                return _appDbContext.Set<T>().AsNoTracking();
            }
            return _appDbContext.Set<T>().AsNoTracking().Where(predicate);
        }

        public void RemoveEntity<T>(T entity) where T : class
        {
            _appDbContext.Set<T>().Remove(entity);
            _appDbContext.SaveChanges();
        }

        public void SaveEntity<T>(T entity) where T : class
        {
            _appDbContext.Set<T>().Add(entity);
            _appDbContext.SaveChanges();
        }

        public void UpdateEntity<T>(T entity) where T : class
        {
            _appDbContext.Set<T>().Update(entity);
            _appDbContext.SaveChanges();
        }
    }
}
