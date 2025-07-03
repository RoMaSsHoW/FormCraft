using FormCraft.Application.Common.Persistance;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace FormCraft.Infrastructure
{
    public class SqlConnectionFacroty : ISqlConnectionFacroty
    {
        private readonly string _connectionString;

        public SqlConnectionFacroty(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("PostgresqlDbConnection")
                ?? throw new ApplicationException("Connection stirng is missing");
        }

        public IDbConnection Create()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}
