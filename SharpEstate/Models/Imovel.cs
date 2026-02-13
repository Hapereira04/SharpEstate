using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharpEstate.Models
{
    public class Imovel
    {
        public int Id { get; set; }

        // --- DADOS PRINCIPAIS ---
        [Required(ErrorMessage = "O título é obrigatório")]
        public string Titulo { get; set; } // Ex: T2 no Centro Histórico

        [Display(Name = "Descrição Pública")]
        public string Descricao { get; set; }

        [Required]
        [Display(Name = "Tipo de Negócio")]
        public TipoNegocio TipoNegocio { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Preço")]
        public decimal Preco { get; set; }

        // --- ÁREAS E DIVISÕES ---
        [Display(Name = "Quartos")]
        public int Quartos { get; set; }

        [Display(Name = "Casas de Banho")]
        public int CasasBanho { get; set; }

        [Display(Name = "Área Útil (m²)")]
        public double AreaUtil { get; set; }

        [Display(Name = "Área Bruta Privativa (m²)")]
        public double? AreaBrutaPrivativa { get; set; }

        [Display(Name = "Estacionamento (Lugares)")]
        public int Estacionamento { get; set; } = 0;

        // --- DETALHES TÉCNICOS ---
        [Display(Name = "Certificado Energético")]
        public CertificadoEnergetico Certificado { get; set; }

        [Display(Name = "Estado")]
        public EstadoImovel Estado { get; set; }

        [Display(Name = "Ano de Construção")]
        public int? AnoConstrucao { get; set; }

        // --- CARACTERÍSTICAS GERAIS ---
        [Display(Name = "Nº de Frentes")]
        public int? NumeroFrentes { get; set; }

        [Display(Name = "Nº de Pisos")]
        public int? NumeroPisos { get; set; }

        [Display(Name = "Vistas")]
        public string? Vistas { get; set; } // Ex: Cidade, Rio

        [Display(Name = "Orientação Solar")]
        public string? OrientacaoSolar { get; set; } // Ex: Nascente/Poente

        // --- LOCALIZAÇÃO ---
        [Required]
        public string Distrito { get; set; } = "Évora"; // Valor padrão

        [Required]
        public string Concelho { get; set; }

        public string Freguesia { get; set; }

        [Display(Name = "Zona / Bairro")]
        public string? Zona { get; set; }

        public string? EnderecoCompleto { get; set; } // Privado, só para o consultor

        // --- GESTÃO ---
        [Display(Name = "Status no Sistema")]
        public StatusSistema Status { get; set; } = StatusSistema.Disponivel;

        public DateTime DataAngariacao { get; set; } = DateTime.Now;

        // --- RELACIONAMENTOS ---

        // Consultor (Quem angariou)
        public string? ConsultorId { get; set; }
        // public virtual IdentityUser Consultor { get; set; } (Opcional, se quiser navegar)

        // Donos (Muitos para Muitos)
        public virtual ICollection<ImovelProprietario> Proprietarios { get; set; } = new List<ImovelProprietario>();

        // Características (Checkboxes: "Perto da Escola", "Água de Companhia", etc.)
        public virtual ICollection<ImovelCaracteristica> Caracteristicas { get; set; } = new List<ImovelCaracteristica>();

        // Fotos
        public virtual ICollection<FotoImovel> Fotos { get; set; } = new List<FotoImovel>();
    }
}