using System.ComponentModel.DataAnnotations;

namespace SharpEstate.Models
{
    public class Consultor
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }
        public string? Email { get; set; }
        public string? Telemovel { get; set; }
        public string? LicencaAMI { get; set; }
        public string? FotoUrl { get; set; }

        // Ligação à tabela de Logins do ASP.NET
        public string? IdentityUserId { get; set; }

        public virtual ICollection<Imovel> ImoveisAngariados { get; set; } = new List<Imovel>();
    }
}