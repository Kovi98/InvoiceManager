using InvoiceManager.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvoiceManager.Model.Data
{
    public static class DbInitializer
    {
        public static void Initialize(InvoiceManagerContext context)
        {
            context.Database.EnsureCreated();

            // Look for any taxes.
            if (context.Taxes.Any())
            {
                return;   // DB has been seeded
            }

            var taxes = new Tax[]
            {
            new Tax{Name = "21 %", Rate = (decimal)0.21},
            new Tax{Name = "15 %", Rate = (decimal)0.15},
            new Tax{Name = "0 %", Rate = (decimal)0}
            };
            foreach (Tax t in taxes)
            {
                context.Taxes.Add(t);
            }
            context.SaveChanges();

            var people = new Person[]
            {
            new Person{Name = "Alois Novák", Street = "Poděbradská", HouseNumber = "32", City = "Šumperk", ZipCode = "78701", State="Česká republika"},
            new Person{Name = "Jiří Novotný", Street = "Dolní Studénky", HouseNumber = "340", City = "Dolní Studénky", ZipCode = "78820", State="Česká republika"},
            new Person{Name = "Prague Labs s.r.o.", Street = "Karlovo náměstí", HouseNumber = "288/17", City = "Praha 2", ZipCode = "12000", State="Česká republika", IC = "04021126", DIC = "CZ04021126"}
            };
            foreach (Person p in people)
            {
                context.People.Add(p);
            }
            context.SaveChanges();
        }
    }
}
