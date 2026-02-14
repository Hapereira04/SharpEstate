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
    public class ImovelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ImovelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Imovels
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Imoveis.Include(i => i.CategoriaImovel).Include(i => i.CertificadoEnergetico).Include(i => i.Consultor).Include(i => i.EstadoImovel).Include(i => i.StatusImovel).Include(i => i.TipoNegocio);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Imovels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imovel = await _context.Imoveis
                .Include(i => i.CategoriaImovel)
                .Include(i => i.CertificadoEnergetico)
                .Include(i => i.Consultor)
                .Include(i => i.EstadoImovel)
                .Include(i => i.StatusImovel)
                .Include(i => i.TipoNegocio)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (imovel == null)
            {
                return NotFound();
            }

            return View(imovel);
        }

        // GET: Imovels/Create
        public IActionResult Create()
        {
            ViewData["CategoriaImovelId"] = new SelectList(_context.CategoriasImovel, "Id", "Nome");
            ViewData["CertificadoEnergeticoId"] = new SelectList(_context.CertificadosEnergeticos, "Id", "Nome");
            ViewData["ConsultorId"] = new SelectList(_context.Consultores, "Id", "Nome");
            ViewData["EstadoImovelId"] = new SelectList(_context.EstadosImovel, "Id", "Nome");
            ViewData["StatusImovelId"] = new SelectList(_context.StatusImoveis, "Id", "Nome");
            ViewData["TipoNegocioId"] = new SelectList(_context.TiposNegocio, "Id", "Nome");
            return View();
        }

        // POST: Imovels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Preco,Descricao,Quartos,CasasBanho,Estacionamento,AreaUtil,AnoConstrucao,NumeroFrentes,Distrito,Concelho,Freguesia,Zona,MoradaExata,NumeroContrato,ObservacoesInternas,ValorComissao,CategoriaImovelId,TipoNegocioId,EstadoImovelId,StatusImovelId,CertificadoEnergeticoId,ConsultorId")] Imovel imovel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(imovel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaImovelId"] = new SelectList(_context.CategoriasImovel, "Id", "Nome", imovel.CategoriaImovelId);
            ViewData["CertificadoEnergeticoId"] = new SelectList(_context.CertificadosEnergeticos, "Id", "Nome", imovel.CertificadoEnergeticoId);
            ViewData["ConsultorId"] = new SelectList(_context.Consultores, "Id", "Nome", imovel.ConsultorId);
            ViewData["EstadoImovelId"] = new SelectList(_context.EstadosImovel, "Id", "Nome", imovel.EstadoImovelId);
            ViewData["StatusImovelId"] = new SelectList(_context.StatusImoveis, "Id", "Nome", imovel.StatusImovelId);
            ViewData["TipoNegocioId"] = new SelectList(_context.TiposNegocio, "Id", "Nome", imovel.TipoNegocioId);
            return View(imovel);
        }

        // GET: Imovels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imovel = await _context.Imoveis.FindAsync(id);
            if (imovel == null)
            {
                return NotFound();
            }
            ViewData["CategoriaImovelId"] = new SelectList(_context.CategoriasImovel, "Id", "Nome", imovel.CategoriaImovelId);
            ViewData["CertificadoEnergeticoId"] = new SelectList(_context.CertificadosEnergeticos, "Id", "Nome", imovel.CertificadoEnergeticoId);
            ViewData["ConsultorId"] = new SelectList(_context.Consultores, "Id", "Nome", imovel.ConsultorId);
            ViewData["EstadoImovelId"] = new SelectList(_context.EstadosImovel, "Id", "Nome", imovel.EstadoImovelId);
            ViewData["StatusImovelId"] = new SelectList(_context.StatusImoveis, "Id", "Nome", imovel.StatusImovelId);
            ViewData["TipoNegocioId"] = new SelectList(_context.TiposNegocio, "Id", "Nome", imovel.TipoNegocioId);
            return View(imovel);
        }

        // POST: Imovels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Preco,Descricao,Quartos,CasasBanho,Estacionamento,AreaUtil,AnoConstrucao,NumeroFrentes,Distrito,Concelho,Freguesia,Zona,MoradaExata,NumeroContrato,ObservacoesInternas,ValorComissao,CategoriaImovelId,TipoNegocioId,EstadoImovelId,StatusImovelId,CertificadoEnergeticoId,ConsultorId")] Imovel imovel)
        {
            if (id != imovel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(imovel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImovelExists(imovel.Id))
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
            ViewData["CategoriaImovelId"] = new SelectList(_context.CategoriasImovel, "Id", "Nome", imovel.CategoriaImovelId);
            ViewData["CertificadoEnergeticoId"] = new SelectList(_context.CertificadosEnergeticos, "Id", "Nome", imovel.CertificadoEnergeticoId);
            ViewData["ConsultorId"] = new SelectList(_context.Consultores, "Id", "Nome", imovel.ConsultorId);
            ViewData["EstadoImovelId"] = new SelectList(_context.EstadosImovel, "Id", "Nome", imovel.EstadoImovelId);
            ViewData["StatusImovelId"] = new SelectList(_context.StatusImoveis, "Id", "Nome", imovel.StatusImovelId);
            ViewData["TipoNegocioId"] = new SelectList(_context.TiposNegocio, "Id", "Nome", imovel.TipoNegocioId);
            return View(imovel);
        }

        // GET: Imovels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imovel = await _context.Imoveis
                .Include(i => i.CategoriaImovel)
                .Include(i => i.CertificadoEnergetico)
                .Include(i => i.Consultor)
                .Include(i => i.EstadoImovel)
                .Include(i => i.StatusImovel)
                .Include(i => i.TipoNegocio)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (imovel == null)
            {
                return NotFound();
            }

            return View(imovel);
        }

        // POST: Imovels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var imovel = await _context.Imoveis.FindAsync(id);
            if (imovel != null)
            {
                _context.Imoveis.Remove(imovel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImovelExists(int id)
        {
            return _context.Imoveis.Any(e => e.Id == id);
        }
    }
}
