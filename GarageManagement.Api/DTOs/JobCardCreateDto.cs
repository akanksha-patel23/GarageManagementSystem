using System;
using System.Collections.Generic;

namespace GarageManagement.Api.DTOs
{
    public class JobCardCreateDto
    {
        public int VehicleID { get; set; }
        public DateTime ScheduledDate { get; set; }
        public int? TechnicianID { get; set; }
        public List<ServiceLineDto> Services { get; set; } = new List<ServiceLineDto>();
        public List<PartLineDto> Parts { get; set; } = new List<PartLineDto>();
    }

    public class ServiceLineDto
    {
        public int ServiceID { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
    }

    public class PartLineDto
    {
        public int PartID { get; set; }
        public int QtyUsed { get; set; }
        public decimal Price { get; set; }
    }

    public class JobCardResultDto
    {
        public int JobCardID { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
    }
}