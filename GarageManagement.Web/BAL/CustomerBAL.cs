using System;
using System.Collections.Generic;
using System.Data;
using GarageManagement.Web.DAL;
using GarageManagement.Web.Entities;

namespace GarageManagement.Web.BAL
{
    /// <summary>
    /// Business rules for Customers. Also owns DataTable -> Customer mapping,
    /// since the DAL (VB.NET, separate project) stays entity-agnostic to avoid
    /// a circular project reference with this project (which owns Entities/).
    /// </summary>
    public class CustomerBAL
    {
        private readonly CustomerDAL _dal = new CustomerDAL();

        public int InsertCustomer(Customer customer)
        {
            Validate(customer);

            int customerId = 0;
            _dal.InsertCustomer(customer.Name, customer.Phone, customer.Email, customer.Address, ref customerId);
            return customerId;
        }

        public void UpdateCustomer(Customer customer)
        {
            if (customer.CustomerID <= 0)
                throw new ArgumentException("Invalid CustomerID.", nameof(customer));

            Validate(customer);
            _dal.UpdateCustomer(customer.CustomerID, customer.Name, customer.Phone, customer.Email, customer.Address);
        }

        public void DeleteCustomer(int customerId)
        {
            if (customerId <= 0)
                throw new ArgumentException("Invalid CustomerID.", nameof(customerId));

            _dal.DeleteCustomer(customerId);
        }

        public Customer GetCustomerById(int customerId)
        {
            DataTable dt = _dal.GetCustomerById(customerId);

            if (dt.Rows.Count == 0)
                return null;

            return MapRowToCustomer(dt.Rows[0]);
        }

        public List<Customer> GetAllCustomers()
        {
            try
            {
                DataTable dt = _dal.GetAllCustomers();
                return MapTableToCustomers(dt);
            }
            catch (Exception ex)
            {
                // Log full exception details for debugging
                System.Diagnostics.EventLog.WriteEntry("Application",
                    $"GetAllCustomers failed: {ex.GetType().FullName}: {ex.Message}\n{ex.StackTrace}",
                    System.Diagnostics.EventLogEntryType.Error);
                throw;
            }
        }

        public List<Customer> SearchCustomers(string searchTerm)
        {
            DataTable dt = _dal.SearchCustomers(searchTerm);
            return MapTableToCustomers(dt);
        }

        // ---- Business rules ----

        private void Validate(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            if (string.IsNullOrWhiteSpace(customer.Name))
                throw new ArgumentException("Customer name is required.");

            if (string.IsNullOrWhiteSpace(customer.Phone))
                throw new ArgumentException("Customer phone is required.");
        }

        // ---- Mapping ----

        private Customer MapRowToCustomer(DataRow row)
        {
            return new Customer
            {
                CustomerID = Convert.ToInt32(row["CustomerID"]),
                Name = row["Name"].ToString(),
                Phone = row["Phone"].ToString(),
                Email = row["Email"] == DBNull.Value ? null : row["Email"].ToString(),
                Address = row["Address"] == DBNull.Value ? null : row["Address"].ToString(),
                RegisteredDate = Convert.ToDateTime(row["RegisteredDate"])
            };
        }

        private List<Customer> MapTableToCustomers(DataTable dt)
        {
            var list = new List<Customer>();
            foreach (DataRow row in dt.Rows)
                list.Add(MapRowToCustomer(row));
            return list;
        }
    }
}