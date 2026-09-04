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

        public int MakeID { get; set; }
        public string MakeName { get; set; }

        public int ModelID { get; set; }
        public string ModelName { get; set; }

        public int? Year { get; set; }

        public int? VehicleTypeID { get; set; }
        public string VehicleTypeName { get; set; }

        public string PhotoPath { get; set; }
    }
    public class VehicleMake
    {
        public int MakeID { get; set; }
        public string MakeName { get; set; }
    }

    public class VehicleModel
    {
        public int ModelID { get; set; }
        public int MakeID { get; set; }
        public string ModelName { get; set; }
    }
}
