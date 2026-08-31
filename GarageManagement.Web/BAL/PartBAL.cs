using System;
using System.Collections.Generic;
using System.Data;
using GarageManagement.Web.DAL;
using GarageManagement.Web.Entities;

namespace GarageManagement.Web.BAL
{
    public class PartBAL
    {
        private readonly PartDAL _dal = new PartDAL();

        public int InsertPart(Part part)
        {
            Validate(part);

            int partId = 0;
            _dal.InsertPart(part.PartName, part.StockQty, part.UnitPrice, ref partId);
            return partId;
        }

        public void UpdatePart(Part part)
        {
            if (part.PartID <= 0)
                throw new ArgumentException("Invalid PartID.", nameof(part));

            Validate(part);
            _dal.UpdatePart(part.PartID, part.PartName, part.UnitPrice);
        }

        public void DeletePart(int partId)
        {
            if (partId <= 0)
                throw new ArgumentException("Invalid PartID.", nameof(partId));

            _dal.DeletePart(partId);
        }

        public List<Part> GetAllParts()
        {
            DataTable dt = _dal.GetAllParts();
            var list = new List<Part>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Part
                {
                    PartID = Convert.ToInt32(row["PartID"]),
                    PartName = row["PartName"].ToString(),
                    StockQty = Convert.ToInt32(row["StockQty"]),
                    UnitPrice = Convert.ToDecimal(row["UnitPrice"])
                });
            }
            return list;
        }

        public void AdjustStock(int partId, int qtyChange)
        {
            if (partId <= 0)
                throw new ArgumentException("Invalid PartID.", nameof(partId));

            if (qtyChange == 0)
                throw new ArgumentException("Quantity change cannot be zero.", nameof(qtyChange));

            _dal.UpdateStock(partId, qtyChange);
        }

        private void Validate(Part part)
        {
            if (part == null)
                throw new ArgumentNullException(nameof(part));

            if (string.IsNullOrWhiteSpace(part.PartName))
                throw new ArgumentException("Part name is required.");

            if (part.UnitPrice < 0)
                throw new ArgumentException("Unit price cannot be negative.");

            if (part.StockQty < 0)
                throw new ArgumentException("Stock quantity cannot be negative.");
        }
    }
}