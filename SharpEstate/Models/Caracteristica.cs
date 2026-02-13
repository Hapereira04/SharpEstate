using System.ComponentModel.DataAnnotations;

namespace SharpEstate.Models
{
    public class Caracteristica
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [Display(Name = "Nome da Característica")]
        public string Nome { get; set; } // Ex: "Piscina", "Alarme", "Banheira"

        // Opcional: Para organizar visualmente no ecrã (Ex: "Segurança", "Conforto", "Exterior")
        [Display(Name = "Categoria")]
        public string? Categoria { get; set; }

        // Relação N:N
        public virtual ICollection<ImovelCaracteristica> ImoveisCaracteristicas { get; set; } = new List<ImovelCaracteristica>();
    }
}