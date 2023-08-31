using Microsoft.Data.SqlClient;
using System.Data;

namespace MeuContexto.Context
{
    public class DapperContext : IDisposable
    {
        public IDbConnection DbConnection { get;}
        public DapperContext(string connectionString)
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
