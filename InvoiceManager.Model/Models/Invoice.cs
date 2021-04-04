using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceManager.Model.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public InvoiceStatus Status { get; set; }
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public DateTime PaymentDue { get; set; }
        public decimal Summary { get; set; }
        public decimal Tax { get; set; }
        public decimal SummaryWithTax { get; set; }

        public Person Customer { get; set; }
        public Person Supplier { get; set; }
        public ICollection<InvoiceItem> Items { get; set; }
    }
    public enum InvoiceStatus
    {
        Created,
        Sent,
        Paid
    }
}
