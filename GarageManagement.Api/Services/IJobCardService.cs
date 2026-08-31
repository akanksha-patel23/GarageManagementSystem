using GarageManagement.Api.DTOs;

namespace GarageManagement.Api.Services
{
    /// <summary>
    /// Interface for JobCard Service
    /// Orchestrates the create transaction: header + lines + stock decrement
    /// </summary>
    public interface IJobCardService
    {
        Task<JobCardResultDto> CreateJobCardAsync(JobCardCreateDto dto);
        Task<IEnumerable<JobCardStatusHistoryDto>> GetStatusHistoryAsync(int jobCardId);
    }
}
