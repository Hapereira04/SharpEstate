using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharpEstate.Models
{
    public class FotoImovel
    {
        public int Id { get; set; }

        // Armazena a imagem em formato binário (bytes)
        [Display(Name = "Ficheiro de Imagem")]
        public byte[] DadosImagem { get; set; }

        // Armazena o tipo do arquivo (ex: "image/jpeg", "image/png")
        // Isso é essencial para o navegador saber como abrir a imagem depois
        public string ContentType { get; set; }

        [Display(Name = "É a Capa?")]
        public bool IsCapa { get; set; }

        // Ligação com o Imóvel
        public int ImovelId { get; set; }
        public virtual Imovel Imovel { get; set; }
    }
}