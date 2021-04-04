using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceManager.Model.Models
{
    public class Tax
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Rate { get; set; }
    }
}
