using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GarageManagement.Web.Entities
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; } // "Admin" or "Customer"
        public bool IsActive { get; set; }
    }
}