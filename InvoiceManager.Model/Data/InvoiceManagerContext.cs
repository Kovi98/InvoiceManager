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
            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.Property(e => e.Summary).HasColumnType("decimal(38, 2)");
                entity.Property(e => e.Tax).HasColumnType("decimal(38, 2)");
                entity.Property(e => e.SummaryWithTax).HasColumnType("decimal(38, 2)");

                entity.HasOne(i => i.Supplier)
                .WithMany(i => i.InvoicesWhereSupplier)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Invoice_Person_SupplierId");

                entity.HasOne(i => i.Customer)
                .WithMany(i => i.InvoicesWhereCustomer)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Invoice_Person_CustomerId");
            });
            modelBuilder.Entity<Invoice>().ToTable("Invoice");

            modelBuilder.Entity<InvoiceItem>(entity =>
            {
                entity.Property(e => e.Price).HasColumnType("decimal(38, 2)");
            });
            modelBuilder.Entity<InvoiceItem>().ToTable("InvoiceItem");
            modelBuilder.Entity<Person>().ToTable("Person");

            modelBuilder.Entity<Tax>(entity =>
            {
                entity.Property(e => e.Rate).HasColumnType("decimal(38, 2)");
            });
            modelBuilder.Entity<Tax>().ToTable("Tax");
        }
    }
}
