using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GarageManagement.Web.Entities
{
    /// <summary>
    /// Entity class for a Vehicle Make (tlkpMake)
    /// </summary>
    public class Make
    {
        public int MakeID { get; set; }
        public string MakeName { get; set; }
    }
}