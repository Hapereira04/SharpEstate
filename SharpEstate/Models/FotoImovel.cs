using System.ComponentModel.DataAnnotations;

namespace SharpEstate.Models
{
    public class FotoImovel
    {
        public int Id { get; set; }

        public byte[] DadosImagem { get; set; }
        public string ContentType { get; set; }

        // --- DRAG & DROP ESTÁ AQUI ---
        [Display(Name = "Ordem de Apresentação")]
        public int Ordem { get; set; }

        // --- LIGAÇÃO AO IMÓVEL ---
        public int ImovelId { get; set; }
        public virtual Imovel? Imovel { get; set; }
    }
}