using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Display(Name = "Payment due date")]
        public DateTime PaymentDue { get; set; }
        public decimal Summary { get; set; }
        public decimal Tax { get; set; }
        [Display(Name = "Summary with tax")]
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
