using GarageManagement.Web.DAL;
using GarageManagement.Web.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace GarageManagement.Web.BAL
{
    public class ServiceCatalogBAL
    {
        private readonly ServiceCatalogDAL _dal = new ServiceCatalogDAL();
        private readonly ServiceCategoryBAL _categoryBal = new ServiceCategoryBAL();

        public int InsertServiceCatalog(ServiceCatalog service)
        {
            Validate(service);

            int serviceId = 0;
            _dal.InsertServiceCatalog(service.CategoryID, service.ServiceName, service.BasePrice, service.EstimatedHours, ref serviceId);
            return serviceId;
        }

        public void UpdateServiceCatalog(ServiceCatalog service)
        {
            if (service.ServiceID <= 0)
                throw new ArgumentException("Invalid ServiceID.", nameof(service));

            Validate(service);
            _dal.UpdateServiceCatalog(service.ServiceID, service.CategoryID, service.ServiceName, service.BasePrice, service.EstimatedHours);
        }

        public void DeleteServiceCatalog(int serviceId)
        {
            if (serviceId <= 0)
                throw new ArgumentException("Invalid ServiceID.", nameof(serviceId));

            _dal.DeleteServiceCatalog(serviceId);
        }

        public List<ServiceCatalog> GetAllServiceCatalog()
        {
            DataTable dt = _dal.GetAllServiceCatalog();

            // Joined in C# rather than adding a new SP purely for a display column.
            var categoryNames = _categoryBal.GetCategoryTree().ToDictionary(c => c.CategoryID, c => c.CategoryName);

            var list = new List<ServiceCatalog>();
            foreach (DataRow row in dt.Rows)
            {
                int categoryId = Convert.ToInt32(row["CategoryID"]);
                list.Add(new ServiceCatalog
                {
                    ServiceID = Convert.ToInt32(row["ServiceID"]),
                    CategoryID = categoryId,
                    ServiceName = row["ServiceName"].ToString(),
                    BasePrice = Convert.ToDecimal(row["BasePrice"]),
                    EstimatedHours = row["EstimatedHours"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(row["EstimatedHours"]),
                    CategoryName = categoryNames.TryGetValue(categoryId, out var name) ? name : "(unknown)"
                });
            }
            return list;
        }

        private void Validate(ServiceCatalog service)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            if (service.CategoryID <= 0)
                throw new ArgumentException("A category must be selected.");

            if (string.IsNullOrWhiteSpace(service.ServiceName))
                throw new ArgumentException("Service name is required.");

            if (service.BasePrice < 0)
                throw new ArgumentException("Base price cannot be negative.");
        }
    }
}
