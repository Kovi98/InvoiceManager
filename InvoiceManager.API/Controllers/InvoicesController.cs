using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InvoiceManager.Model.Data;
using InvoiceManager.Model.Models;

namespace InvoiceManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly InvoiceManagerContext _context;

        public InvoicesController(InvoiceManagerContext context)
        {
            _context = context;
        }

        // GET: api/Invoices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetUnpaidInvoices()
        {
            return await _context.Invoices.Where(x => x.Status != InvoiceStatus.Paid).ToListAsync();
        }

        // PUT: api/Invoices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PayInvoice(int id)
        {
            if (!InvoiceExists(id))
                return NotFound();
            var invoice = _context.Invoices.Find(id);
            if (invoice.Status == InvoiceStatus.Paid)
            {
                return BadRequest();
            }
            invoice.Status = InvoiceStatus.Paid;
            _context.Entry(invoice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoices.Any(e => e.Id == id);
        }
    }
}
