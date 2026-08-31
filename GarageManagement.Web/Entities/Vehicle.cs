namespace GarageManagement.Web.Entities
{
    /// <summary>
    /// Entity class for Vehicle
    /// </summary>
    public class Vehicle
    {
        public int VehicleID { get; set; }
        public int CustomerID { get; set; }
        public string RegNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int? Year { get; set; }
        public string VehicleType { get; set; }
        public string PhotoPath { get; set; }
    }
}
