using System.ComponentModel.DataAnnotations;

namespace SharpEstate.Models
{
    public class EstadoImovel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Estado do Imóvel")]
        public string Nome { get; set; } // Ex: Novo, Usado

        public virtual ICollection<Imovel> Imoveis { get; set; } = new List<Imovel>();
    }
}