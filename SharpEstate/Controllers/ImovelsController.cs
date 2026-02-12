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
            return View(await _context.Imoveis.ToListAsync());
        }

        // GET: Imovels/Details/5
        // GET: Imoveis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imovel = await _context.Imoveis
                .Include(i => i.Fotos)
                .Include(i => i.Caracteristicas)
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
            return View();
        }

        // POST: Imovels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Imovel imovel)
        {
            // 1. Evitar que o C# bloqueie o salvamento por causa de listas vazias
            ModelState.Remove("Fotos");
            ModelState.Remove("Proprietarios");
            ModelState.Remove("Caracteristicas");

            if (ModelState.IsValid)
            {
                // 2. Apanhar as fotos DIRETAMENTE do formulário HTML (Método à prova de bala)
                var ficheiros = HttpContext.Request.Form.Files;

                if (ficheiros.Count > 0)
                {
                    foreach (var ficheiro in ficheiros)
                    {
                        // Verifica se é mesmo uma imagem e se não está vazia
                        if (ficheiro.Length > 0 && ficheiro.ContentType.StartsWith("image/"))
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                // Copia o ficheiro para a memória
                                await ficheiro.CopyToAsync(memoryStream);

                                // Cria o objeto Foto
                                var novaFoto = new FotoImovel
                                {
                                    DadosImagem = memoryStream.ToArray(),
                                    ContentType = ficheiro.ContentType,
                                    IsCapa = imovel.Fotos.Count == 0 // A primeira a entrar vira Capa
                                };

                                // Adiciona ao imóvel
                                imovel.Fotos.Add(novaFoto);
                            }
                        }
                    }
                }

                // 3. Gravar na Base de Dados
                _context.Add(imovel);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Se faltou preencher algo (ex: Título), volta para a tela de erro
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
            return View(imovel);
        }

        // POST: Imovels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descricao,TipoNegocio,Preco,Quartos,CasasBanho,AreaUtil,AreaBrutaPrivativa,Estacionamento,Certificado,Estado,AnoConstrucao,NumeroFrentes,NumeroPisos,Vistas,OrientacaoSolar,Distrito,Concelho,Freguesia,Zona,EnderecoCompleto,Status,DataAngariacao,ConsultorId")] Imovel imovel)
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
