using GarageManagement.Web.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using GarageManagement.Web.DAL;

namespace GarageManagement.Web.BAL
{
    public class VehicleBAL
    {
        private readonly VehicleDAL _dal = new VehicleDAL();

        public int InsertVehicle(Vehicle vehicle)
        {
            Validate(vehicle);

            int vehicleId = 0;
            _dal.InsertVehicle(vehicle.CustomerID, vehicle.RegNumber, vehicle.Make, vehicle.Model,
                vehicle.Year, vehicle.VehicleType, vehicle.PhotoPath, ref vehicleId);
            return vehicleId;
        }

        public void UpdateVehicle(Vehicle vehicle)
        {
            if (vehicle.VehicleID <= 0)
                throw new ArgumentException("Invalid VehicleID.", nameof(vehicle));

            Validate(vehicle);
            _dal.UpdateVehicle(vehicle.VehicleID, vehicle.RegNumber, vehicle.Make, vehicle.Model,
                vehicle.Year, vehicle.VehicleType, vehicle.PhotoPath);
        }

        public void DeleteVehicle(int vehicleId)
        {
            if (vehicleId <= 0)
                throw new ArgumentException("Invalid VehicleID.", nameof(vehicleId));

            _dal.DeleteVehicle(vehicleId);
        }

        public Vehicle GetVehicleById(int vehicleId)
        {
            DataTable dt = _dal.GetVehicleById(vehicleId);
            return dt.Rows.Count == 0 ? null : MapRowToVehicle(dt.Rows[0]);
        }

        public List<Vehicle> GetAllVehicles()
        {
            return MapTableToVehicles(_dal.GetAllVehicles());
        }

        // ---- Business rules ----

        private void Validate(Vehicle vehicle)
        {
            if (vehicle == null)
                throw new ArgumentNullException(nameof(vehicle));

            if (vehicle.CustomerID <= 0)
                throw new ArgumentException("A customer must be selected.");

            if (string.IsNullOrWhiteSpace(vehicle.RegNumber))
                throw new ArgumentException("Registration number is required.");

            if (string.IsNullOrWhiteSpace(vehicle.Make))
                throw new ArgumentException("Make is required.");

            if (string.IsNullOrWhiteSpace(vehicle.Model))
                throw new ArgumentException("Model is required.");

            if (vehicle.Year.HasValue && (vehicle.Year < 1980 || vehicle.Year > DateTime.Now.Year + 1))
                throw new ArgumentException("Year is out of range.");
        }

        // ---- Mapping ----

        private Vehicle MapRowToVehicle(DataRow row)
        {
            return new Vehicle
            {
                VehicleID = Convert.ToInt32(row["VehicleID"]),
                CustomerID = Convert.ToInt32(row["CustomerID"]),
                RegNumber = row["RegNumber"].ToString(),
                Make = row["Make"].ToString(),
                Model = row["Model"].ToString(),
                Year = row["Year"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["Year"]),
                VehicleType = row["VehicleType"] == DBNull.Value ? null : row["VehicleType"].ToString(),
                PhotoPath = row.Table.Columns.Contains("PhotoPath") && row["PhotoPath"] != DBNull.Value
                    ? row["PhotoPath"].ToString()
                    : null
            };
        }

        private List<Vehicle> MapTableToVehicles(DataTable dt)
        {
            var list = new List<Vehicle>();
            foreach (DataRow row in dt.Rows)
                list.Add(MapRowToVehicle(row));
            return list;
        }
    }
}
