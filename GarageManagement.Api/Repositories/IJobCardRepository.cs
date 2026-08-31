using GarageManagement.Api.DTOs;

namespace GarageManagement.Api.Repositories
{
    /// <summary>
    /// Interface for JobCard Repository
    /// spInsertJobCard, spInsertJobCardService, spInsertJobCardPart, spUpdatePartStock, spGetStatusHistoryByJobCard
    /// All inside one transaction for create
    /// </summary>
    public interface IJobCardRepository
    {
        Task<JobCardResultDto> CreateJobCardAsync(JobCardCreateDto dto, decimal totalAmount);
        Task<IEnumerable<JobCardStatusHistoryDto>> GetStatusHistoryAsync(int jobCardId);
    }
}
