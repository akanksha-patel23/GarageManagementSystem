using GarageManagement.Web.DAL;
using GarageManagement.Web.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace GarageManagement.Web.BAL
{
    public class VehicleBAL
    {
        private readonly VehicleDAL _dal = new VehicleDAL();

        public int InsertVehicle(Vehicle vehicle)
        {
            Validate(vehicle);

            int vehicleId = 0;
            _dal.InsertVehicle(vehicle.CustomerID, vehicle.RegNumber, vehicle.MakeID, vehicle.ModelID,
                vehicle.Year, vehicle.VehicleTypeID, vehicle.PhotoPath, ref vehicleId);
            return vehicleId;
        }

        public void UpdateVehicle(Vehicle vehicle)
        {
            if (vehicle.VehicleID <= 0)
                throw new ArgumentException("Invalid VehicleID.", nameof(vehicle));

            Validate(vehicle);
            _dal.UpdateVehicle(vehicle.VehicleID, vehicle.RegNumber, vehicle.MakeID, vehicle.ModelID,
                vehicle.Year, vehicle.VehicleTypeID, vehicle.PhotoPath);
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

        // ---- Lookup tables (tlkpMake / tlkpModel / tlkpVehicleType) ----

        public List<VehicleMake> GetAllMakes()
        {
            DataTable dt = _dal.GetAllMakes();
            var list = new List<VehicleMake>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new VehicleMake
                {
                    MakeID = Convert.ToInt32(row["MakeID"]),
                    MakeName = row["MakeName"].ToString()
                });
            }
            return list;
        }

        public List<VehicleModel> GetModelsByMake(int makeId)
        {
            DataTable dt = _dal.GetModelsByMake(makeId);
            var list = new List<VehicleModel>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new VehicleModel
                {
                    ModelID = Convert.ToInt32(row["ModelID"]),
                    MakeID = Convert.ToInt32(row["MakeID"]),
                    ModelName = row["ModelName"].ToString()
                });
            }
            return list;
        }

        public List<VehicleType> GetAllVehicleTypes()
        {
            DataTable dt = _dal.GetAllVehicleTypes();
            var list = new List<VehicleType>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new VehicleType
                {
                    VehicleTypeID = Convert.ToInt32(row["VehicleTypeID"]),
                    VehicleTypeName = row["VehicleTypeName"].ToString()
                });
            }
            return list;
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

            if (vehicle.MakeID <= 0)
                throw new ArgumentException("Make is required.");

            if (vehicle.ModelID <= 0)
                throw new ArgumentException("Model is required.");

            if (vehicle.Year.HasValue && (vehicle.Year < 1980 || vehicle.Year > DateTime.Now.Year + 1))
                throw new ArgumentException("Year is out of range.");
        }

        // ---- Mapping ----
        // Column names below match spGetAllVehicles/spGetVehicleById exactly:
        // MakeID, MakeName, ModelID, ModelName, VehicleTypeID, VehicleTypeName
        // (joined from tlkpMake/tlkpModel/tlkpVehicleType) — NOT "Make"/"Model"/
        // "VehicleType", which don't exist as columns anymore.

        private Vehicle MapRowToVehicle(DataRow row)
        {
            return new Vehicle
            {
                VehicleID = Convert.ToInt32(row["VehicleID"]),
                CustomerID = Convert.ToInt32(row["CustomerID"]),
                RegNumber = row["RegNumber"].ToString(),
                MakeID = Convert.ToInt32(row["MakeID"]),
                MakeName = row["MakeName"].ToString(),
                ModelID = Convert.ToInt32(row["ModelID"]),
                ModelName = row["ModelName"].ToString(),
                Year = row["Year"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["Year"]),
                VehicleTypeID = row["VehicleTypeID"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["VehicleTypeID"]),
                VehicleTypeName = row["VehicleTypeName"] == DBNull.Value ? null : row["VehicleTypeName"].ToString(),
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
