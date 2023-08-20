using Microsoft.Data.SqlClient;
using System.Data;

namespace MeuContexto.Context
{
    public class DbSession : IDisposable
    {
        public IDbConnection DbConnection { get;}
        public DbSession(string connectionString)
        {
            DbConnection = new SqlConnection(connectionString);

            DbConnection.Open();
        }
        public void Dispose()
        {
            DbConnection?.Dispose();
        }
    }
}
