using System.ComponentModel.DataAnnotations.Schema;

namespace SharpEstate.Models
{
    public class ImovelProprietario
    {
        public int ImovelId { get; set; }
        public virtual Imovel Imovel { get; set; }

        public int ClienteId { get; set; }
        public virtual Cliente Cliente { get; set; }
    }
}