using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SharpEstate.Data;
using SharpEstate.Models;

namespace SharpEstate.Controllers
{
    public class InteressesCompraController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InteressesCompraController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: InteressesCompra
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.InteressesCompra.Include(i => i.Cliente);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: InteressesCompra/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interesseCompra = await _context.InteressesCompra
                .Include(i => i.Cliente)
                .FirstOrDefaultAsync(m => m.ClienteId == id);
            if (interesseCompra == null)
            {
                return NotFound();
            }

            return View(interesseCompra);
        }

        // GET: InteressesCompra/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome");
            return View();
        }

        // POST: InteressesCompra/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClienteId,PrecoMaximo,Tipologias,Concelhos,Urgencia,DataRegisto")] InteresseCompra interesseCompra)
        {
            if (ModelState.IsValid)
            {
                _context.Add(interesseCompra);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome", interesseCompra.ClienteId);
            return View(interesseCompra);
        }

        // GET: InteressesCompra/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interesseCompra = await _context.InteressesCompra.FindAsync(id);
            if (interesseCompra == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome", interesseCompra.ClienteId);
            return View(interesseCompra);
        }

        // POST: InteressesCompra/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClienteId,PrecoMaximo,Tipologias,Concelhos,Urgencia,DataRegisto")] InteresseCompra interesseCompra)
        {
            if (id != interesseCompra.ClienteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(interesseCompra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InteresseCompraExists(interesseCompra.ClienteId))
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
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome", interesseCompra.ClienteId);
            return View(interesseCompra);
        }

        // GET: InteressesCompra/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interesseCompra = await _context.InteressesCompra
                .Include(i => i.Cliente)
                .FirstOrDefaultAsync(m => m.ClienteId == id);
            if (interesseCompra == null)
            {
                return NotFound();
            }

            return View(interesseCompra);
        }

        // POST: InteressesCompra/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var interesseCompra = await _context.InteressesCompra.FindAsync(id);
            if (interesseCompra != null)
            {
                _context.InteressesCompra.Remove(interesseCompra);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InteresseCompraExists(int id)
        {
            return _context.InteressesCompra.Any(e => e.ClienteId == id);
        }
    }
}
