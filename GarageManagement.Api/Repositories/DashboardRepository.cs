using Dapper;
using GarageManagement.Api.DTOs;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GarageManagement.Api.Repositories
{
    /// <summary>
    /// Implementation of Dashboard Repository
    /// spGetDashboardStats via QueryMultiple
    /// </summary>
    public class DashboardRepository : IDashboardRepository
    {
        private readonly string _connectionString;

        public DashboardRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("GarageManagementDB");
        }

        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            using var conn = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@ErrorID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using var multi = await conn.QueryMultipleAsync(
                "spGetDashboardStats", parameters, commandType: CommandType.StoredProcedure);

            var stats = new DashboardStatsDto
            {
                MonthlyRevenue = (await multi.ReadAsync<MonthlyRevenueDto>()).AsList(),
                StatusDistribution = (await multi.ReadAsync<StatusDistributionDto>()).AsList()
            };

            int errorId = parameters.Get<int>("@ErrorID");
            if (errorId != 0)
                throw new ApplicationException($"Database operation failed. Reference ID: {errorId}");

            return stats;
        }
    }
}
