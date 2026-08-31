using System;

namespace GarageManagement.Web.Entities
{
    /// <summary>
    /// Entity class for Invoice
    /// </summary>
    public class Invoice
    {
        public int InvoiceID { get; set; }
        public int JobCardID { get; set; }
        public DateTime GeneratedDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentStatus { get; set; }
    }
}
