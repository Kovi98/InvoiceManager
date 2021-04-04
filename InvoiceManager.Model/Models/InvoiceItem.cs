using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceManager.Model.Models
{
    public class InvoiceItem
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int TaxRateId { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }

        public decimal Tax
        {
            get
            {
                return Count * Price * TaxRate.Rate;
            }
        }
        public decimal PriceSummary
        {
            get
            {
                return Price + Tax;
            }
        }
        public Invoice Invoice { get; set; }
        public Tax TaxRate { get; set; }
    }
}
