using GarageManagement.Api.DTOs;
using GarageManagement.Api.Repositories;

namespace GarageManagement.Api.Services
{
    /// <summary>
    /// Implementation of JobCard Service
    /// Orchestrates the create transaction: header + lines + stock decrement
    /// </summary>
    public class JobCardService : IJobCardService
    {
        private readonly IJobCardRepository _repository;

        public JobCardService(IJobCardRepository repository)
        {
            _repository = repository;
        }

        public Task<JobCardResultDto> CreateJobCardAsync(JobCardCreateDto dto)
        {
            Validate(dto);

            decimal totalAmount = dto.Services.Sum(s => s.Qty * s.Price)
                                 + dto.Parts.Sum(p => p.QtyUsed * p.Price);

            return _repository.CreateJobCardAsync(dto, totalAmount);
        }

        public Task<IEnumerable<JobCardStatusHistoryDto>> GetStatusHistoryAsync(int jobCardId)
        {
            if (jobCardId <= 0)
                throw new ArgumentException("Invalid JobCardID.", nameof(jobCardId));

            return _repository.GetStatusHistoryAsync(jobCardId);
        }

        private void Validate(JobCardCreateDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (dto.VehicleID <= 0)
                throw new ArgumentException("A vehicle must be selected.");

            if (dto.Services.Count == 0 && dto.Parts.Count == 0)
                throw new ArgumentException("At least one service or part is required.");

            foreach (var s in dto.Services)
            {
                if (s.Qty <= 0)
                    throw new ArgumentException($"Invalid quantity for ServiceID {s.ServiceID}.");
            }

            foreach (var p in dto.Parts)
            {
                if (p.QtyUsed <= 0)
                    throw new ArgumentException($"Invalid quantity for PartID {p.PartID}.");
            }
        }
    }
}
