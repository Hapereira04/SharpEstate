using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SharpEstate.Data;
using SharpEstate.Models;

namespace SharpEstate.Controllers
{
    public class ConsultorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        // 1. Criamos a variável para o Gestor de Utilizadores do Identity
        private readonly UserManager<IdentityUser> _userManager;

        // 2. Adicionamos ao construtor
        public ConsultorsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Consultors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Consultores.ToListAsync());
        }

        // GET: Consultors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultor = await _context.Consultores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (consultor == null)
            {
                return NotFound();
            }

            return View(consultor);
        }

        // GET: Consultors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Consultors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Email,Telemovel,LicencaAMI")] Consultor consultor, IFormFile? fotoUpload)
        {
            // Limpamos do ModelState os campos que vamos preencher manualmente, para não dar erro
            ModelState.Remove("FotoPerfil");
            ModelState.Remove("ContentTypeFoto");
            ModelState.Remove("IdentityUserId");
            ModelState.Remove("ImoveisAngariados");

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(consultor.Email))
                {
                    ModelState.AddModelError("Email", "O Email é obrigatório para podermos criar o Login do consultor.");
                    return View(consultor);
                }

                // PASSO 1: Verificar se este email já tem conta no site
                var userExistente = await _userManager.FindByEmailAsync(consultor.Email);
                if (userExistente != null)
                {
                    ModelState.AddModelError("Email", "Já existe uma conta no sistema com este email.");
                    return View(consultor);
                }

                // PASSO 2: Criar o Login no Cofre do Identity
                var novoUser = new IdentityUser
                {
                    UserName = consultor.Email,
                    Email = consultor.Email,
                    EmailConfirmed = true // Assumimos que o Admin não se enganou no email
                };

                // Criamos a conta com uma Password Provisória (O Consultor depois pode mudar)
                var resultadoIdentity = await _userManager.CreateAsync(novoUser, "Sharp123!");

                if (resultadoIdentity.Succeeded)
                {
                    // Dá o cargo de Consultor a este novo Login
                    await _userManager.AddToRoleAsync(novoUser, "Consultor");

                    // PASSO 3: A PONTE DE LIGAÇÃO! Guardamos o ID do Login no nosso perfil do Consultor
                    consultor.IdentityUserId = novoUser.Id;

                    // PASSO 4: Tratar da Foto de Perfil (Bytes e RGPD)
                    if (fotoUpload != null && fotoUpload.Length > 0)
                    {
                        if (fotoUpload.ContentType.StartsWith("image/"))
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                await fotoUpload.CopyToAsync(memoryStream);
                                consultor.FotoPerfil = memoryStream.ToArray();
                                consultor.ContentTypeFoto = fotoUpload.ContentType;
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("FotoPerfil", "O ficheiro enviado não é uma imagem válida.");
                            await _userManager.DeleteAsync(novoUser); // Se a foto falhar, apagamos o login para não deixar "lixo"
                            return View(consultor);
                        }
                    }

                    // PASSO 5: Guardar a ficha do Consultor na nossa base de dados!
                    _context.Add(consultor);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Se falhar a criação do Login (ex: password fraca), mostramos os erros
                    foreach (var erro in resultadoIdentity.Errors)
                    {
                        ModelState.AddModelError(string.Empty, erro.Description);
                    }
                }
            }

            return View(consultor);
        }

        // GET: Consultors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultor = await _context.Consultores.FindAsync(id);
            if (consultor == null)
            {
                return NotFound();
            }
            return View(consultor);
        }

        // POST: Consultors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Email,Telemovel,LicencaAMI,FotoUrl,IdentityUserId")] Consultor consultor)
        {
            if (id != consultor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(consultor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsultorExists(consultor.Id))
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
            return View(consultor);
        }

        // GET: Consultors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultor = await _context.Consultores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (consultor == null)
            {
                return NotFound();
            }

            return View(consultor);
        }

        // POST: Consultors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var consultor = await _context.Consultores.FindAsync(id);
            if (consultor != null)
            {
                _context.Consultores.Remove(consultor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConsultorExists(int id)
        {
            return _context.Consultores.Any(e => e.Id == id);
        }
    }
}
