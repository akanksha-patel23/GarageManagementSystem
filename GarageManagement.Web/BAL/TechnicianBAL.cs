using GarageManagement.Web.DAL;
using GarageManagement.Web.Entities;
using System;
using System.Collections.Generic;
using System.Data;

namespace GarageManagement.Web.BAL
{
    public class TechnicianBAL
    {
        private readonly TechnicianDAL _dal = new TechnicianDAL();

        public int InsertTechnician(Technician technician)
        {
            Validate(technician);

            int technicianId = 0;
            _dal.InsertTechnician(technician.Name, technician.Specialization, technician.ContactNumber, ref technicianId);
            return technicianId;
        }

        public void UpdateTechnician(Technician technician)
        {
            if (technician.TechnicianID <= 0)
                throw new ArgumentException("Invalid TechnicianID.", nameof(technician));

            Validate(technician);
            _dal.UpdateTechnician(technician.TechnicianID, technician.Name, technician.Specialization, technician.ContactNumber);
        }

        public void DeleteTechnician(int technicianId)
        {
            if (technicianId <= 0)
                throw new ArgumentException("Invalid TechnicianID.", nameof(technicianId));

            _dal.DeleteTechnician(technicianId);
        }

        public List<Technician> GetAllTechnicians()
        {
            DataTable dt = _dal.GetAllTechnicians();
            var list = new List<Technician>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Technician
                {
                    TechnicianID = Convert.ToInt32(row["TechnicianID"]),
                    Name = row["Name"].ToString(),
                    Specialization = row["Specialization"] == DBNull.Value ? null : row["Specialization"].ToString(),
                    ContactNumber = row["ContactNumber"] == DBNull.Value ? null : row["ContactNumber"].ToString()
                });
            }
            return list;
        }

        private void Validate(Technician technician)
        {
            if (technician == null)
                throw new ArgumentNullException(nameof(technician));

            if (string.IsNullOrWhiteSpace(technician.Name))
                throw new ArgumentException("Technician name is required.");
        }
    }
}
