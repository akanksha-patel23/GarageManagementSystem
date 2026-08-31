using System;

namespace GarageManagement.Web.Entities
{
    /// <summary>
    /// Entity class for Customer
    /// </summary>
    public class Customer
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime RegisteredDate { get; set; }
    }
}
