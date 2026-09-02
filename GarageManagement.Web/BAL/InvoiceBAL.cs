using System;
using System.Collections.Generic;
using System.Data;
using GarageManagement.Web.Entities;
using GarageManagement.Web.DAL;

namespace GarageManagement.Web.BAL
{
    public class InvoiceBAL
    {
        private readonly InvoiceDAL _dal = new InvoiceDAL();

        private static readonly string[] ValidPaymentStatuses = { "Pending", "Paid", "Refunded" };

        public int InsertInvoice(Invoice invoice)
        {
            if (invoice == null)
                throw new ArgumentNullException(nameof(invoice));

            if (invoice.JobCardID <= 0)
                throw new ArgumentException("A job card must be specified.");

            if (invoice.TotalAmount < 0)
                throw new ArgumentException("Total amount cannot be negative.");

            int invoiceId = 0;
            _dal.InsertInvoice(invoice.JobCardID, invoice.TotalAmount, invoice.PaymentMode, ref invoiceId);
            return invoiceId;
        }

        public void UpdatePaymentStatus(int invoiceId, string paymentStatus)
        {
            if (invoiceId <= 0)
                throw new ArgumentException("Invalid InvoiceID.", nameof(invoiceId));

            if (Array.IndexOf(ValidPaymentStatuses, paymentStatus) < 0)
                throw new ArgumentException($"'{paymentStatus}' is not a valid payment status.");

            _dal.UpdatePaymentStatus(invoiceId, paymentStatus);
        }

        public List<Invoice> GetAllInvoices()
        {
            DataTable dt = _dal.GetAllInvoices();
            var list = new List<Invoice>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Invoice
                {
                    InvoiceID = Convert.ToInt32(row["InvoiceID"]),
                    JobCardID = Convert.ToInt32(row["JobCardID"]),
                    GeneratedDate = Convert.ToDateTime(row["GeneratedDate"]),
                    TotalAmount = Convert.ToDecimal(row["TotalAmount"]),
                    PaymentMode = row["PaymentMode"] == DBNull.Value ? null : row["PaymentMode"].ToString(),
                    PaymentStatus = row["PaymentStatus"].ToString()
                });
            }
            return list;
        }

        public DataTable GetAllJobCardsForDropdown()
        {
            return _dal.GetAllJobCardsForDropdown();
        }
    }
}
