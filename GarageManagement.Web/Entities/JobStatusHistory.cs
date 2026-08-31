using System;

namespace GarageManagement.Web.Entities
{
    /// <summary>
    /// Entity class for JobStatusHistory
    /// </summary>
    public class JobStatusHistory
    {
        public int HistoryID { get; set; }
        public int JobCardID { get; set; }
        public string Status { get; set; }
        public DateTime ChangedDate { get; set; }
        public string Remarks { get; set; }
    }
}
