using System;

namespace GarageManagement.Web.Entities
{
    /// <summary>
    /// Entity class for JobCard
    /// </summary>
    public class JobCard
    {
        public int JobCardId { get; set; }
        public int VehicleId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public int? TechnicianId { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
