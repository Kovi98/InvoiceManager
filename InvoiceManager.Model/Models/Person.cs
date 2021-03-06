using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceManager.Model.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IC { get; set; }
        public string DIC { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }

        public ICollection<Invoice> InvoicesWhereSupplier { get; set; }
        public ICollection<Invoice> InvoicesWhereCustomer { get; set; }
    }
}
