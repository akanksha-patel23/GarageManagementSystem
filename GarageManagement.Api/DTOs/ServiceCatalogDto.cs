namespace GarageManagement.Api.DTOs
{
    /// <summary>
    /// DTO for ServiceCatalog
    /// </summary>
    public class ServiceCatalogDto
    {
        public int ServiceID { get; set; }
        public int CategoryID { get; set; }
        public string ServiceName { get; set; }
        public decimal BasePrice { get; set; }
        public decimal? EstimatedHours { get; set; }
    }
}
