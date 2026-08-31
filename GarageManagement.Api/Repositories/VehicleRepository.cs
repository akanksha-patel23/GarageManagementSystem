using GarageManagement.Api.DTOs;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;

namespace GarageManagement.Api.Repositories
{
    /// <summary>
    /// Dapper repository � parameterized calls only, per 05-api-conventions.md.
    /// Backs GET /api/vehicles/{customerId}, used exclusively by the vehicle
    /// dropdown in JobCardWizard.aspx (Flow C in 10-canonical-flows.md).
    /// VehicleList.aspx (the admin grid) never calls this � it uses Flow A
    /// (VehicleBAL -> VehicleDAL) instead.
    /// </summary>
    public class VehicleRepository : IVehicleRepository
    {
        private readonly string _connectionString;

        public VehicleRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("GarageManagementDB");
        }

        public async Task<IEnumerable<VehicleDto>> GetVehiclesByCustomerAsync(int customerId)
        {
            using var conn = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@CustomerID", customerId);
            parameters.Add("@ErrorID", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

            var result = await conn.QueryAsync<VehicleDto>(
                "spGetVehiclesByCustomer",
                parameters,
                commandType: System.Data.CommandType.StoredProcedure);

            int errorId = parameters.Get<int>("@ErrorID");
            if (errorId != 0)
                throw new ApplicationException($"Database operation failed. Reference ID: {errorId}");

            return result;
        }
    }
}
