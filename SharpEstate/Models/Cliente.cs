using System.ComponentModel.DataAnnotations;

namespace SharpEstate.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório")]
        [Display(Name = "Telemóvel")]
        public string Telefone { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Display(Name = "Observações Internas")]
        public string? Observacoes { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        // --- RELACIONAMENTOS ---

        // Lista de imóveis que este cliente possui (Tabela Intermédia)
        public virtual ICollection<ImovelProprietario> ImoveisProprietario { get; set; } = new List<ImovelProprietario>();

        // Ficha de comprador (O que ele procura)
        public virtual InteresseCompra? InteresseCompra { get; set; }
    }
}