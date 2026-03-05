using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SharpEstate.Data;
using SharpEstate.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace SharpEstate.Controllers
{
    [Authorize]
    public class ImovelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ImovelsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Imoveis
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Imoveis
                .Include(i => i.CategoriaImovel)
                .Include(i => i.EstadoImovel)
                .Include(i => i.TipoNegocio)
                .Include(i => i.Fotos);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Imovels/Details/5
        [AllowAnonymous] // Permite que qualquer pessoa da internet veja o anúncio!
                         // GET: Imoveis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var imovel = await _context.Imoveis
                .Include(i => i.CategoriaImovel)
                .Include(i => i.EstadoImovel)
                .Include(i => i.TipoNegocio)
                .Include(i => i.CertificadoEnergetico)
                .Include(i => i.Consultor)
                .Include(i => i.Fotos) // Carrega as fotos
                                       // ESTA É A PARTE QUE FALTA PARA O CCTV APARECER:
                .Include(i => i.Caracteristicas)
                    .ThenInclude(ic => ic.Caracteristica)
                        .ThenInclude(c => c.GrupoCaracteristica)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (imovel == null) return NotFound();

            // Vai descobrir quem é o proprietário atual deste imóvel
            var proprietarioAtual = await _context.ImoveisProprietarios.FirstOrDefaultAsync(ip => ip.ImovelId == id);

            // Carrega a lista de Clientes e já deixa selecionado o dono atual!
            ViewData["ListaClientes"] = new SelectList(_context.Clientes, "Id", "Nome", proprietarioAtual?.ClienteId);

            return View(imovel);
        }

        // GET: Imovels/Create
        [Authorize(Roles = "Admin,Consultor")]
        public async Task<IActionResult> Create()
        {
            // Catálogos normais
            ViewData["CategoriaImovelId"] = new SelectList(_context.CategoriasImovel, "Id", "Nome");
            ViewData["TipoNegocioId"] = new SelectList(_context.TiposNegocio, "Id", "Nome");
            ViewData["EstadoImovelId"] = new SelectList(_context.EstadosImovel, "Id", "Nome");
            ViewData["StatusImovelId"] = new SelectList(_context.StatusImoveis, "Id", "Nome");
            ViewData["CertificadoEnergeticoId"] = new SelectList(_context.CertificadosEnergeticos, "Id", "Nome");
            ViewData["ListaClientes"] = new SelectList(_context.Clientes, "Id", "Nome");

            // MAGIA: Buscar os Grupos com as suas Características para desenhar os Checkboxes
            ViewBag.GruposComCaracteristicas = await _context.GruposCaracteristicas
                .Include(g => g.Caracteristicas)
                .ToListAsync();

            return View();
        }

        // POST: Imovels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Consultor")]
        // Adicionámos "List<int> selectedCaracteristicas" para apanhar as Checkboxes do HTML!
        public async Task<IActionResult> Create([Bind("Id,Titulo,Preco,Descricao,Quartos,CasasBanho,Estacionamento,AreaUtil,AreaBruta,Piso,AnoConstrucao,NumeroFrentes,Distrito,Concelho,Freguesia,Zona,MoradaExata,NumeroContrato,ObservacoesInternas,ValorComissao,CategoriaImovelId,TipoNegocioId,EstadoImovelId,StatusImovelId,CertificadoEnergeticoId")] Imovel imovel, int? clienteProprietarioId, List<int> selectedCaracteristicas, List<IFormFile> fotosUpload)
        {
            ModelState.Remove("ConsultorId");

            if (ModelState.IsValid)
            {
                // 1. Associa o Consultor
                var user = await _userManager.GetUserAsync(User);
                var consultorLogado = await _context.Consultores.FirstOrDefaultAsync(c => c.IdentityUserId == user.Id);
                if (consultorLogado != null) imovel.ConsultorId = consultorLogado.Id;

                // 2. Gravar o Imóvel (Para gerar o ID do Imóvel)
                _context.Add(imovel);
                await _context.SaveChangesAsync();

                // 3. Associar o Proprietário
                if (clienteProprietarioId.HasValue)
                {
                    _context.ImoveisProprietarios.Add(new ImovelProprietario { ImovelId = imovel.Id, ClienteId = clienteProprietarioId.Value });
                    await _context.SaveChangesAsync();
                }

                // 4. Gravar as Características Extras (Ar Condicionado, Piscina, etc.)
                if (selectedCaracteristicas != null && selectedCaracteristicas.Any())
                {
                    foreach (var caracId in selectedCaracteristicas)
                    {
                        _context.ImoveisCaracteristicas.Add(new ImovelCaracteristica
                        {
                            ImovelId = imovel.Id,
                            CaracteristicaId = caracId
                        });
                    }
                    await _context.SaveChangesAsync();
                }

                // 5. O UPLOAD MÚLTIPLO DE FOTOS ORDENADAS
                if (fotosUpload != null && fotosUpload.Count > 0)
                {
                    int ordemCounter = 1; // Para guardar a ordem do Drag & Drop!
                    foreach (var formFile in fotosUpload)
                    {
                        if (formFile.Length > 0 && formFile.ContentType.StartsWith("image/"))
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                await formFile.CopyToAsync(memoryStream);
                                var novaFoto = new FotoImovel
                                {
                                    ImovelId = imovel.Id,
                                    DadosImagem = memoryStream.ToArray(),
                                    ContentType = formFile.ContentType,
                                    Ordem = ordemCounter // 1, 2, 3...
                                };
                                _context.Fotos.Add(novaFoto);
                                ordemCounter++;
                            }
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            // Se falhar a validação, recarrega as listas todas
            ViewData["CategoriaImovelId"] = new SelectList(_context.CategoriasImovel, "Id", "Nome", imovel.CategoriaImovelId);
            ViewData["TipoNegocioId"] = new SelectList(_context.TiposNegocio, "Id", "Nome", imovel.TipoNegocioId);
            ViewData["EstadoImovelId"] = new SelectList(_context.EstadosImovel, "Id", "Nome", imovel.EstadoImovelId);
            ViewData["StatusImovelId"] = new SelectList(_context.StatusImoveis, "Id", "Nome", imovel.StatusImovelId);
            ViewData["CertificadoEnergeticoId"] = new SelectList(_context.CertificadosEnergeticos, "Id", "Nome", imovel.CertificadoEnergeticoId);
            ViewData["ListaClientes"] = new SelectList(_context.Clientes, "Id", "Nome", clienteProprietarioId);
            ViewBag.GruposComCaracteristicas = await _context.GruposCaracteristicas.Include(g => g.Caracteristicas).ToListAsync();

            return View(imovel);
        }

        // GET: Imovels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            // 1. Ir buscar o imóvel e CARREGAR as Fotos e Características
            var imovel = await _context.Imoveis
                .Include(i => i.Fotos)
                .Include(i => i.Caracteristicas)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (imovel == null) return NotFound();

            // 2. Dropdowns Normais
            ViewData["CategoriaImovelId"] = new SelectList(_context.CategoriasImovel, "Id", "Nome", imovel.CategoriaImovelId);
            ViewData["CertificadoEnergeticoId"] = new SelectList(_context.CertificadosEnergeticos, "Id", "Nome", imovel.CertificadoEnergeticoId);
            ViewData["ConsultorId"] = new SelectList(_context.Consultores, "Id", "Nome", imovel.ConsultorId);
            ViewData["EstadoImovelId"] = new SelectList(_context.EstadosImovel, "Id", "Nome", imovel.EstadoImovelId);
            ViewData["StatusImovelId"] = new SelectList(_context.StatusImoveis, "Id", "Nome", imovel.StatusImovelId);
            ViewData["TipoNegocioId"] = new SelectList(_context.TiposNegocio, "Id", "Nome", imovel.TipoNegocioId);

            // 3. Buscar os Grupos e Características para os Checkboxes
            ViewBag.GruposComCaracteristicas = await _context.GruposCaracteristicas
                .Include(g => g.Caracteristicas)
                .ToListAsync();

            // 4. PROTEÇÃO ANTI-CRASH: O '?' garante que se não houver características, ele devolve uma lista vazia sem crachar o site!
            ViewBag.CaracteristicasAtuais = imovel.Caracteristicas?.Select(c => c.CaracteristicaId).ToList() ?? new List<int>();

            return View(imovel);
        }

        // POST: Imovels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Preco,Descricao,Quartos,CasasBanho,Estacionamento,AreaUtil,AreaBruta,Piso,AnoConstrucao,NumeroFrentes,Distrito,Concelho,Freguesia,Zona,MoradaExata,NumeroContrato,ObservacoesInternas,ValorComissao,CategoriaImovelId,TipoNegocioId,EstadoImovelId,StatusImovelId,CertificadoEnergeticoId,ConsultorId")] Imovel imovel, List<int> selectedCaracteristicas, List<IFormFile> novasFotos, int? clienteProprietarioId)
        {
            if (id != imovel.Id) return NotFound();

            // LER A ORDEM À FORÇA DIRETAMENTE DO HTML! (À prova de falhas)
            string existingPhotoOrder = Request.Form["existingPhotoOrder"];

            ModelState.Remove("Consultor");
            ModelState.Remove("CategoriaImovel");
            ModelState.Remove("TipoNegocio");
            ModelState.Remove("EstadoImovel");
            ModelState.Remove("StatusImovel");
            ModelState.Remove("CertificadoEnergetico");
            ModelState.Remove("Fotos");
            ModelState.Remove("Caracteristicas");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(imovel);

                    // 1. ATUALIZAR AS CARACTERÍSTICAS
                    var caracteristicasAntigas = _context.ImoveisCaracteristicas.Where(ic => ic.ImovelId == id);
                    _context.ImoveisCaracteristicas.RemoveRange(caracteristicasAntigas);

                    if (selectedCaracteristicas != null && selectedCaracteristicas.Any())
                    {
                        foreach (var caracId in selectedCaracteristicas)
                        {
                            _context.ImoveisCaracteristicas.Add(new ImovelCaracteristica { ImovelId = imovel.Id, CaracteristicaId = caracId });
                        }
                    }

                    // 2. ATUALIZAR O PROPRIETÁRIO
                    var propAtual = _context.ImoveisProprietarios.FirstOrDefault(ip => ip.ImovelId == imovel.Id);
                    if (propAtual != null) _context.ImoveisProprietarios.Remove(propAtual);
                    if (clienteProprietarioId.HasValue) _context.ImoveisProprietarios.Add(new ImovelProprietario { ImovelId = imovel.Id, ClienteId = clienteProprietarioId.Value });

                    // 3. A MAGIA UNIFICADA: GRAVAR A ORDEM MISTA
                    if (!string.IsNullOrEmpty(existingPhotoOrder))
                    {
                        var itensOrdem = existingPhotoOrder.Split(',').Select(s => s.Trim()).ToList();
                        int ordemAtual = 1;

                        var listaNovasFotos = novasFotos?.ToList() ?? new List<IFormFile>();

                        foreach (var item in itensOrdem)
                        {
                            if (item.StartsWith("old_"))
                            {
                                if (int.TryParse(item.Replace("old_", ""), out int fotoId))
                                {
                                    var foto = await _context.Fotos.FindAsync(fotoId);
                                    if (foto != null && foto.ImovelId == id)
                                    {
                                        foto.Ordem = ordemAtual;
                                        _context.Update(foto);
                                        ordemAtual++;
                                    }
                                }
                            }
                            else if (item.StartsWith("new_"))
                            {
                                if (int.TryParse(item.Replace("new_", ""), out int fileIndex))
                                {
                                    if (fileIndex >= 0 && fileIndex < listaNovasFotos.Count)
                                    {
                                        var formFile = listaNovasFotos[fileIndex];
                                        if (formFile.Length > 0 && formFile.ContentType.StartsWith("image/"))
                                        {
                                            using (var memoryStream = new MemoryStream())
                                            {
                                                await formFile.CopyToAsync(memoryStream);
                                                var novaFoto = new FotoImovel
                                                {
                                                    ImovelId = imovel.Id,
                                                    DadosImagem = memoryStream.ToArray(),
                                                    ContentType = formFile.ContentType,
                                                    Ordem = ordemAtual
                                                };
                                                _context.Fotos.Add(novaFoto);
                                                ordemAtual++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImovelExists(imovel.Id))
                    {
                        TempData["MensagemErro"] = $"O imóvel REF: SHARP-{imovel.Id:D5} já foi apagado por outro utilizador.";
                        return RedirectToAction(nameof(Index));
                    }
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            // Se falhar a validação, recarrega tudo
            ViewData["CategoriaImovelId"] = new SelectList(_context.CategoriasImovel, "Id", "Nome", imovel.CategoriaImovelId);
            ViewData["CertificadoEnergeticoId"] = new SelectList(_context.CertificadosEnergeticos, "Id", "Nome", imovel.CertificadoEnergeticoId);
            ViewData["ConsultorId"] = new SelectList(_context.Consultores, "Id", "Nome", imovel.ConsultorId);
            ViewData["EstadoImovelId"] = new SelectList(_context.EstadosImovel, "Id", "Nome", imovel.EstadoImovelId);
            ViewData["StatusImovelId"] = new SelectList(_context.StatusImoveis, "Id", "Nome", imovel.StatusImovelId);
            ViewData["TipoNegocioId"] = new SelectList(_context.TiposNegocio, "Id", "Nome", imovel.TipoNegocioId);
            var proprietarioAtual = _context.ImoveisProprietarios.FirstOrDefault(ip => ip.ImovelId == imovel.Id);
            ViewData["ListaClientes"] = new SelectList(_context.Clientes, "Id", "Nome", proprietarioAtual?.ClienteId);
            ViewBag.GruposComCaracteristicas = await _context.GruposCaracteristicas.Include(g => g.Caracteristicas).ToListAsync();
            ViewBag.CaracteristicasAtuais = selectedCaracteristicas ?? new List<int>();

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
