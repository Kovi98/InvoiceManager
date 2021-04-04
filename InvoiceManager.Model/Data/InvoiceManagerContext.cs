using InvoiceManager.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceManager.Model.Data
{
    public class InvoiceManagerContext : DbContext
    {
        public InvoiceManagerContext(DbContextOptions<InvoiceManagerContext> options) : base(options)
        {
        }

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Invoice>().ToTable("Invoice");
            modelBuilder.Entity<InvoiceItem>().ToTable("InvoiceItem");
            modelBuilder.Entity<Person>().ToTable("Person");
            modelBuilder.Entity<Tax>().ToTable("Tax");
        }
    }
}
