using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InvoiceManager.Model.Data;
using InvoiceManager.Model.Models;
using Microsoft.AspNetCore.JsonPatch;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetUnpaidInvoices()
        {
            return await _context.Invoices.Where(x => x.Status != InvoiceStatus.Paid).ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayInvoice(int id)
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

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchInvoice(int? id, [FromBody] JsonPatchDocument<Invoice> patch)
        {
            if (id.HasValue && InvoiceExists(id.Value) && patch != null)
            {
                var invoice = await _context.Invoices.FindAsync(id);
                patch.ApplyTo(invoice, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

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

                return Ok(invoice);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoices.Any(e => e.Id == id);
        }
    }
}
