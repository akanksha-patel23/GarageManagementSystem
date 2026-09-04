using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GarageManagement.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("GarageManagementDB");
        }

        public async Task<UserRecord> GetUserByUsernameAsync(string username)
        {
            using var conn = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@Username", username);
            parameters.Add("@ErrorID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = await conn.QueryFirstOrDefaultAsync<UserRecord>(
                "spGetUserByUsername", parameters, commandType: CommandType.StoredProcedure);

            int errorId = parameters.Get<int>("@ErrorID");
            if (errorId != 0)
                throw new System.ApplicationException($"Database operation failed. Reference ID: {errorId}");

            return result;
        }
    }
}
