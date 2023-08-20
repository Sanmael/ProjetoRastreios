using Dapper;
using DapperExtensions;
using Entities.Interfaces;
using MeuContexto.Context;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MeuContexto.Repositorie
{
    public class DapperRepository : IRepository
    {
        public static string connectionString = "Data Source=DESKTOP-AUTOI40\\SQLEXPRESS;Initial Catalog=NomeDoBancoDeDados;Integrated Security=True;Encrypt=False";

        private readonly DbSession _dbSession;

        public DapperRepository(DbSession dbSession)
        {
            _dbSession = dbSession;
        }
        public static IDbConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public async Task<T> GetEntityAsync<T>(Func<T, bool> predicate) where T : class
        {
            IDbConnection db = GetConnection();

            using (db)
            {
                var teste = await db.GetListAsync<T>();

                return teste.FirstOrDefault(predicate);
            }
        }

        public async Task<List<T>> GetEntityByProcedure<T>(string proc, KeyValuePair<string, object>? parameters = null) where T : class
        {
            using (var db = GetConnection())
            {
                if (parameters != null)
                {
                    return db.Query<T>(proc, parameters).ToList();
                }
                return db.Query<T>(proc).ToList();
            }
        }

        public async Task<List<T>> GetEntitys<T>(Func<T, bool>? predicate = null) where T : class
        {
            IDbConnection db = GetConnection();

            using (db)
            {
                if (predicate != null)
                {
                    return db.GetList<T>(predicate).ToList();
                }

                return db.GetList<T>().ToList();
            }
        }

        public Task RemoveEntityAsync<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public async Task SaveEntityAsync<T>(T entity) where T : class
        {
            IDbConnection db = GetConnection();

            using (db)
            {
                await db.InsertAsync<T>(entity);
            }
        }

        public async Task UpdateEntityAsync<T>(T entity) where T : class
        {
            IDbConnection db = GetConnection();

            using (db)
            {
                await db.UpdateAsync<T>(entity);
            }
        }
    }
}
