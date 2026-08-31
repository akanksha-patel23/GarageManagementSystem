namespace GarageManagement.Api.DTOs
{
    /// <summary>
    /// DTO for Vehicle
    /// </summary>
    public class VehicleDto
    {
        public int VehicleID { get; set; }
        public int CustomerID { get; set; }
        public string RegNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int? Year { get; set; }
        public string VehicleType { get; set; }
    }
}
