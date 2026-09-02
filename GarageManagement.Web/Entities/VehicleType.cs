using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GarageManagement.Web.Entities
{
    /// <summary>
    /// Entity class for a Vehicle Type (tlkpVehicleType)
    /// </summary>
    public class VehicleType
    {
        public int VehicleTypeID { get; set; }
        public string VehicleTypeName { get; set; }
    }
}