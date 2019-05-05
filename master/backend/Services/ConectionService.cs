using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CoreNg.Services
{
    public interface IConectionService
    {
        DbConnection GetAnOpenConnection();
    }

    public class ConectionService : IConectionService
    {
        private IConfiguration _configuration;

        public ConectionService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbConnection GetAnOpenConnection()
        {
            var connectionString = _configuration.GetConnectionString("Database");

            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}
