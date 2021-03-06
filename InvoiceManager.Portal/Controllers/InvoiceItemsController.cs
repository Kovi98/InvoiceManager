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
    public class InvoiceItemsController : Controller
    {
        private readonly InvoiceManagerContext _context;

        public InvoiceItemsController(InvoiceManagerContext context)
        {
            _context = context;
        }

        // GET: InvoiceItems
        public async Task<IActionResult> Index(int? id)
        {
            var invoiceManagerContext = _context.InvoiceItems.Include(i => i.Invoice).Include(i => i.TaxRate).Where(i => i.InvoiceId == id);
            if (id != null && id > 0)
            {
                invoiceManagerContext.Where(x => x.InvoiceId == id);
                ViewData["ParentId"] = id;
            }
            return View(await invoiceManagerContext.ToListAsync());
        }

        // GET: InvoiceItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["ParentId"] = id;
            var invoiceItem = await _context.InvoiceItems
                .Include(i => i.Invoice)
                .Include(i => i.TaxRate)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoiceItem == null)
            {
                return NotFound();
            }

            return View(invoiceItem);
        }

        // GET: InvoiceItems/Create
        public IActionResult Create(int? id)
        {
            ViewData["ParentId"] = id;
            ViewData["TaxRateId"] = new SelectList(_context.Taxes, "Id", "Name");
            return View();
        }

        // POST: InvoiceItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,InvoiceId,TaxRateId,Name,Count,Price")] InvoiceItem invoiceItem)
        {
            invoiceItem.TaxRate = _context.Taxes.Find(invoiceItem.TaxRateId);
            if (ModelState.IsValid)
            {
                _context.Add(invoiceItem);
                await _context.SaveChangesAsync();
                var invoice = _context.Invoices.Include(x => x.Items).ToList().Find(x => x.Id == invoiceItem.InvoiceId);
                invoice.Recalculate();
                _context.Update(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "InvoiceItems", new { id = invoiceItem.InvoiceId });
            }
            ViewData["TaxRateId"] = new SelectList(_context.Taxes, "Id", "Name", invoiceItem.TaxRateId);
            return View(invoiceItem);
        }

        // GET: InvoiceItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceItem = await _context.InvoiceItems.FindAsync(id);
            ViewData["ParentId"] = invoiceItem.InvoiceId;
            if (invoiceItem == null)
            {
                return NotFound();
            }
            ViewData["TaxRateId"] = new SelectList(_context.Taxes, "Id", "Name", invoiceItem.TaxRateId);
            return View(invoiceItem);
        }

        // POST: InvoiceItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InvoiceId,TaxRateId,Name,Count,Price")] InvoiceItem invoiceItem)
        {
            if (id != invoiceItem.Id)
            {
                return NotFound();
            }
            invoiceItem.TaxRate = _context.Taxes.Find(invoiceItem.TaxRateId);
            ViewData["ParentId"] = invoiceItem.InvoiceId;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoiceItem);
                    await _context.SaveChangesAsync();
                    var invoice = _context.Invoices.Include(x => x.Items).ToList().Find(x => x.Id == invoiceItem.InvoiceId);
                    invoice.Recalculate();
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceItemExists(invoiceItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "InvoiceItems", new { id = invoiceItem.InvoiceId });
            }
            ViewData["TaxRateId"] = new SelectList(_context.Taxes, "Id", "Name", invoiceItem.TaxRateId);
            return View(invoiceItem);
        }

        // GET: InvoiceItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceItem = await _context.InvoiceItems
                .Include(i => i.Invoice)
                .Include(i => i.TaxRate)
                .FirstOrDefaultAsync(m => m.Id == id);
            ViewData["ParentId"] = invoiceItem.InvoiceId;
            if (invoiceItem == null)
            {
                return NotFound();
            }

            return View(invoiceItem);
        }

        // POST: InvoiceItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoiceItem = await _context.InvoiceItems.FindAsync(id);
            _context.InvoiceItems.Remove(invoiceItem);
            await _context.SaveChangesAsync();
            var invoice = _context.Invoices.Include(x => x.Items).ToList().Find(x => x.Id == invoiceItem.InvoiceId);
            invoice.Recalculate();
            _context.Update(invoice);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "InvoiceItems", new { id = invoiceItem.InvoiceId });
        }

        private bool InvoiceItemExists(int id)
        {
            return _context.InvoiceItems.Any(e => e.Id == id);
        }
    }
}
