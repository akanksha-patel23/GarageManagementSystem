using Dapper;
using GarageManagement.Api.DTOs;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GarageManagement.Api.Repositories
{
    /// <summary>
    /// Implementation of Service Catalog Repository
    /// spGetServicesByCategory
    /// </summary>
    public class ServiceCatalogRepository : IServiceCatalogRepository
    {
        private readonly string _connectionString;

        public ServiceCatalogRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("GarageManagementDB");
        }

        public async Task<IEnumerable<ServiceCatalogDto>> GetServicesByCategoryAsync(int categoryId)
        {
            using var conn = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@CategoryID", categoryId);
            parameters.Add("@ErrorID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = await conn.QueryAsync<ServiceCatalogDto>(
                "spGetServicesByCategory",
                parameters,
                commandType: CommandType.StoredProcedure);

            int errorId = parameters.Get<int>("@ErrorID");
            if (errorId != 0)
                throw new ApplicationException($"Database operation failed. Reference ID: {errorId}");

            return result;
        }
    }
}
