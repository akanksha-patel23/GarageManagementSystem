using System;
using System.Collections.Generic;
using System.Data;
using GarageManagement.Web.DAL;
using GarageManagement.Web.Entities;

namespace GarageManagement.Web.BAL
{
    /// <summary>
    /// Business Access Layer for JobCard operations
    /// Contains status transition rules, e.g. can't go Completed -> Pending
    /// </summary>
    public class JobCardBAL
    {
        private readonly JobCardDAL _dal = new JobCardDAL();

        private static readonly string[] ValidStatuses = { "Pending", "InProgress", "Completed", "Cancelled" };

        public List<JobCard> GetAllJobCards()
        {
            DataTable dt = _dal.GetAllJobCards();
            var list = new List<JobCard>();
            foreach (DataRow row in dt.Rows)
                list.Add(MapRowToJobCard(row));
            return list;
        }

        public JobCard GetJobCardById(int jobCardId)
        {
            DataTable dt = _dal.GetJobCardById(jobCardId);
            return dt.Rows.Count == 0 ? null : MapRowToJobCard(dt.Rows[0]);
        }

        public void UpdateStatus(int jobCardId, string newStatus, string remarks)
        {
            if (jobCardId <= 0)
                throw new ArgumentException("Invalid JobCardID.", nameof(jobCardId));

            if (Array.IndexOf(ValidStatuses, newStatus) < 0)
                throw new ArgumentException($"'{newStatus}' is not a valid status.", nameof(newStatus));

            _dal.UpdateJobCardStatus(jobCardId, newStatus, remarks);
        }

        public List<JobStatusHistory> GetStatusHistory(int jobCardId)
        {
            DataTable dt = _dal.GetStatusHistoryByJobCard(jobCardId);
            var list = new List<JobStatusHistory>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new JobStatusHistory
                {
                    HistoryID = Convert.ToInt32(row["HistoryID"]),
                    JobCardID = Convert.ToInt32(row["JobCardID"]),
                    Status = row["Status"].ToString(),
                    ChangedDate = Convert.ToDateTime(row["ChangedDate"]),
                    Remarks = row["Remarks"] == DBNull.Value ? null : row["Remarks"].ToString()
                });
            }
            return list;
        }

        /// <summary>Next status in the Pending -> InProgress -> Completed sequence, or null if terminal.</summary>
        public string GetNextStatus(string currentStatus)
        {
            switch (currentStatus)
            {
                case "Pending": return "InProgress";
                case "InProgress": return "Completed";
                default: return null; // Completed and Cancelled are terminal
            }
        }

        private JobCard MapRowToJobCard(DataRow row)
        {
            return new JobCard
            {
                JobCardId = Convert.ToInt32(row["JobCardID"]),
                VehicleId = Convert.ToInt32(row["VehicleID"]),
                ScheduledDate = Convert.ToDateTime(row["ScheduledDate"]),
                TechnicianId = row["TechnicianID"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["TechnicianID"]),
                Status = row["Status"].ToString(),
                TotalAmount = Convert.ToDecimal(row["TotalAmount"])
            };
        }
    }
}
