using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharpEstate.Models
{
    public class InteresseCompra
    {
        [Key, ForeignKey("Cliente")]
        public int ClienteId { get; set; }
        public virtual Cliente Cliente { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Orçamento Máximo")]
        public decimal PrecoMaximo { get; set; }

        [Display(Name = "Tipologias (Ex: T2,T3)")]
        public string? Tipologias { get; set; }

        [Display(Name = "Concelhos de Interesse")]
        public string? Concelhos { get; set; }

        [Display(Name = "Urgência")]
        public NivelUrgencia Urgencia { get; set; } = NivelUrgencia.Media;

        public DateTime DataRegisto { get; set; } = DateTime.Now;
    }
}