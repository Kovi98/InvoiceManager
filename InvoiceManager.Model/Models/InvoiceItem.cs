using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Display(Name = "Price all")]
        public decimal PriceAll
        {
            get
            {
                return Price * Count;
            }
        }
        public decimal Tax
        {
            get
            {
                return PriceAll * TaxRate?.Rate ?? 0;
            }
        }
        [Display(Name = "Price with tax")]
        public decimal PriceSummary
        {
            get
            {
                return PriceAll + Tax;
            }
        }
        public Invoice Invoice { get; set; }
        [Display(Name = "Tax rate")]
        public Tax TaxRate { get; set; }
    }
}
