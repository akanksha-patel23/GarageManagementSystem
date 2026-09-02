using Dapper;
using GarageManagement.Api.DTOs;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GarageManagement.Api.Repositories
{
    /// <summary>
    /// Implementation of JobCard Repository
    /// spInsertJobCard, spInsertJobCardService, spInsertJobCardPart, spUpdatePartStock, spGetStatusHistoryByJobCard
    /// All inside one transaction for create
    /// </summary>
    public class JobCardRepository : IJobCardRepository
    {
        private readonly string _connectionString;

        public JobCardRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("GarageManagementDB");
        }

        public async Task<JobCardResultDto> CreateJobCardAsync(JobCardCreateDto dto, decimal totalAmount)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            using var transaction = conn.BeginTransaction();

            try
            {
                // 1. Header (also seeds JobStatusHistory with 'Pending' inside the SP itself)
                var headerParams = new DynamicParameters();
                headerParams.Add("@VehicleID", dto.VehicleID);
                headerParams.Add("@ScheduledDate", dto.ScheduledDate);
                headerParams.Add("@TechnicianID", dto.TechnicianID);
                headerParams.Add("@JobCardID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                headerParams.Add("@ErrorID", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await conn.ExecuteAsync("spInsertJobCard", headerParams,
                    transaction: transaction, commandType: CommandType.StoredProcedure);
                CheckErrorId(headerParams);

                int jobCardId = headerParams.Get<int>("@JobCardID");

                // 2. Service lines
                foreach (var service in dto.Services)
                {
                    var serviceParams = new DynamicParameters();
                    serviceParams.Add("@JobCardID", jobCardId);
                    serviceParams.Add("@ServiceID", service.ServiceID);
                    serviceParams.Add("@Qty", service.Qty);
                    serviceParams.Add("@Price", service.Price);
                    serviceParams.Add("@ErrorID", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    await conn.ExecuteAsync("spInsertJobCardService", serviceParams,
                        transaction: transaction, commandType: CommandType.StoredProcedure);
                    CheckErrorId(serviceParams);
                }

                // 3. Part lines + stock decrement (same transaction — one failure rolls back everything)
                foreach (var part in dto.Parts)
                {
                    var partParams = new DynamicParameters();
                    partParams.Add("@JobCardID", jobCardId);
                    partParams.Add("@PartID", part.PartID);
                    partParams.Add("@QtyUsed", part.QtyUsed);
                    partParams.Add("@Price", part.Price);
                    partParams.Add("@ErrorID", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    await conn.ExecuteAsync("spInsertJobCardPart", partParams,
                        transaction: transaction, commandType: CommandType.StoredProcedure);
                    CheckErrorId(partParams);

                    var stockParams = new DynamicParameters();
                    stockParams.Add("@PartID", part.PartID);
                    stockParams.Add("@QtyChange", -part.QtyUsed); // negative decrements stock
                    stockParams.Add("@ErrorID", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    await conn.ExecuteAsync("spUpdatePartStock", stockParams,
                        transaction: transaction, commandType: CommandType.StoredProcedure);
                    CheckErrorId(stockParams); // throws if stock was insufficient (see spUpdatePartStock's RAISERROR)
                }

                // 4. Persist the computed total — spInsertJobCard hardcodes TotalAmount = 0
                //    at creation, so without this the row stays at $0.00 forever even
                //    though this method returns the correct total in its response.
                var totalParams = new DynamicParameters();
                totalParams.Add("@JobCardID", jobCardId);
                totalParams.Add("@TotalAmount", totalAmount);
                totalParams.Add("@ErrorID", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await conn.ExecuteAsync("spUpdateJobCardTotal", totalParams,
                    transaction: transaction, commandType: CommandType.StoredProcedure);
                CheckErrorId(totalParams);

                transaction.Commit();

                return new JobCardResultDto
                {
                    JobCardID = jobCardId,
                    TotalAmount = totalAmount,
                    Status = "Pending"
                };
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<IEnumerable<JobCardStatusHistoryDto>> GetStatusHistoryAsync(int jobCardId)
        {
            using var conn = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@JobCardID", jobCardId);
            parameters.Add("@ErrorID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = await conn.QueryAsync<JobCardStatusHistoryDto>(
                "spGetStatusHistoryByJobCard", parameters, commandType: CommandType.StoredProcedure);

            CheckErrorId(parameters);
            return result;
        }

        private void CheckErrorId(DynamicParameters parameters)
        {
            int errorId = parameters.Get<int>("@ErrorID");
            if (errorId != 0)
                throw new ApplicationException($"Database operation failed. Reference ID: {errorId}");
        }
    }
}
