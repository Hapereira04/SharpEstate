using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SharpEstate.Data;

namespace SharpEstate.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context; // A nossa Base de Dados

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        // O MODELO DO FORMULÁRIO (Agora totalmente livre de Requireds escondidos!)
        public class InputModel
        {
            [Display(Name = "Nome Completo")]
            public string? Nome { get; set; } // Retirado o [Required] e adicionado o ?

            [Phone(ErrorMessage = "Número de telemóvel inválido.")]
            [Display(Name = "Telemóvel")]
            public string? Telemovel { get; set; } // Adicionado o ?

            [Display(Name = "NIF")]
            public string? NIF { get; set; } // Adicionado o ?

            [Display(Name = "Licença AMI")]
            public string? LicencaAMI { get; set; } // Adicionado o ?

            [Display(Name = "Nova Foto de Perfil")]
            public IFormFile? FotoUpload { get; set; } // AQUI ESTAVA O ERRO! Adicionado o ?

            // Variáveis de controlo para a View saber o que mostrar
            public string? FotoBase64Atual { get; set; }
            public bool IsConsultor { get; set; }
            public bool IsCliente { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            Username = userName;

            Input = new InputModel();

            // Vai à base de dados descobrir QUEM é esta pessoa
            var consultor = await _context.Consultores.FirstOrDefaultAsync(c => c.IdentityUserId == user.Id);
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.IdentityUserId == user.Id);

            if (consultor != null)
            {
                Input.IsConsultor = true;
                Input.Nome = consultor.Nome;
                Input.Telemovel = consultor.Telemovel;
                Input.LicencaAMI = consultor.LicencaAMI;

                if (consultor.FotoPerfil != null)
                    Input.FotoBase64Atual = $"data:{consultor.ContentTypeFoto};base64,{Convert.ToBase64String(consultor.FotoPerfil)}";
            }
            else if (cliente != null)
            {
                Input.IsCliente = true;
                Input.Nome = cliente.Nome;
                Input.Telemovel = cliente.Telemovel;
                Input.NIF = cliente.NIF;

                if (cliente.FotoPerfil != null)
                    Input.FotoBase64Atual = $"data:{cliente.ContentTypeFoto};base64,{Convert.ToBase64String(cliente.FotoPerfil)}";
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) { return NotFound($"Erro ao carregar o utilizador com ID '{_userManager.GetUserId(User)}'."); }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) { return NotFound($"Erro ao carregar o utilizador com ID '{_userManager.GetUserId(User)}'."); }

            // MAGIA: Limpar do validador os campos que servem apenas para a View, 
            // para não bloquearem a gravação da foto!
            ModelState.Remove("Input.FotoBase64Atual");
            ModelState.Remove("Input.IsConsultor");
            ModelState.Remove("Input.IsCliente");

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var consultor = await _context.Consultores.FirstOrDefaultAsync(c => c.IdentityUserId == user.Id);
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.IdentityUserId == user.Id);

            byte[] novaFotoBytes = null;
            string novoContentType = null;

            // Converter a foto nova em Bytes (agora já não é bloqueado!)
            if (Input.FotoUpload != null && Input.FotoUpload.Length > 0)
            {
                if (Input.FotoUpload.ContentType.StartsWith("image/"))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await Input.FotoUpload.CopyToAsync(memoryStream);
                        novaFotoBytes = memoryStream.ToArray();
                        novoContentType = Input.FotoUpload.ContentType;
                    }
                }
            }

            // GRAVAÇÃO: Atualizar apenas quem for encontrado
            if (consultor != null)
            {
                consultor.Nome = Input.Nome;
                consultor.Telemovel = Input.Telemovel;
                if (Input.LicencaAMI != null) consultor.LicencaAMI = Input.LicencaAMI;

                if (novaFotoBytes != null)
                {
                    consultor.FotoPerfil = novaFotoBytes;
                    consultor.ContentTypeFoto = novoContentType;
                }
                _context.Update(consultor);
            }
            else if (cliente != null)
            {
                cliente.Nome = Input.Nome;
                cliente.Telemovel = Input.Telemovel;
                if (Input.NIF != null) cliente.NIF = Input.NIF;

                if (novaFotoBytes != null)
                {
                    cliente.FotoPerfil = novaFotoBytes;
                    cliente.ContentTypeFoto = novoContentType;
                }
                _context.Update(cliente);
            }

            await _context.SaveChangesAsync();
            await _signInManager.RefreshSignInAsync(user);

            // A NOSSA MENSAGEM DE SUCESSO PERSONALIZADA
            TempData["MensagemSucesso"] = "O seu perfil foi atualizado com sucesso!";

            return RedirectToPage();
        }
    }
}