using System;
using System.Collections.Generic;
using System.Data;
using GarageManagement.Web.DAL;
using GarageManagement.Web.Entities;

namespace GarageManagement.Web.BAL
{
    public class ServiceCategoryBAL
    {
        private readonly ServiceCategoryDAL _dal = new ServiceCategoryDAL();

        public int InsertServiceCategory(ServiceCategory category)
        {
            Validate(category);

            int categoryId = 0;
            _dal.InsertServiceCategory(category.ParentCategoryID, category.CategoryName, ref categoryId);
            return categoryId;
        }

        public void UpdateServiceCategory(ServiceCategory category)
        {
            if (category.CategoryID <= 0)
                throw new ArgumentException("Invalid CategoryID.", nameof(category));

            Validate(category);
            _dal.UpdateServiceCategory(category.CategoryID, category.ParentCategoryID, category.CategoryName);
        }

        public void DeleteServiceCategory(int categoryId)
        {
            if (categoryId <= 0)
                throw new ArgumentException("Invalid CategoryID.", nameof(categoryId));

            _dal.DeleteServiceCategory(categoryId);
        }

        public List<ServiceCategory> GetCategoryTree()
        {
            DataTable dt = _dal.GetServiceCategoryTree();
            var list = new List<ServiceCategory>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new ServiceCategory
                {
                    CategoryID = Convert.ToInt32(row["CategoryID"]),
                    ParentCategoryID = row["ParentCategoryID"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["ParentCategoryID"]),
                    CategoryName = row["CategoryName"].ToString(),
                    Depth = Convert.ToInt32(row["Depth"])
                });
            }
            return list;
        }

        private void Validate(ServiceCategory category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            if (string.IsNullOrWhiteSpace(category.CategoryName))
                throw new ArgumentException("Category name is required.");
        }
    }
}