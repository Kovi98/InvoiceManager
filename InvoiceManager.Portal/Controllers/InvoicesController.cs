using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InvoiceManager.Model.Data;
using InvoiceManager.Model.Models;

namespace InvoiceManager.Portal.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly InvoiceManagerContext _context;

        public InvoicesController(InvoiceManagerContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
            var invoiceManagerContext = _context.Invoices.Include(i => i.Customer).Include(i => i.Supplier);
            return View(await invoiceManagerContext.ToListAsync());
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Supplier)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.People, "Id", "Name");
            ViewData["SupplierId"] = new SelectList(_context.People, "Id", "Name");
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Created,Status,CustomerId,SupplierId,PaymentDue,Summary,Tax,SummaryWithTax")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                invoice.Created = DateTime.Now;
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.People, "Id", "Name", invoice.CustomerId);
            ViewData["SupplierId"] = new SelectList(_context.People, "Id", "Name", invoice.SupplierId);
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.People, "Id", "Name", invoice.CustomerId);
            ViewData["SupplierId"] = new SelectList(_context.People, "Id", "Name", invoice.SupplierId);
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Created,Status,CustomerId,SupplierId,PaymentDue,Summary,Tax,SummaryWithTax")] Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.People, "Id", "Name", invoice.CustomerId);
            ViewData["SupplierId"] = new SelectList(_context.People, "Id", "Name", invoice.SupplierId);
            return View(invoice);
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoices.Any(e => e.Id == id);
        }
    }
}
