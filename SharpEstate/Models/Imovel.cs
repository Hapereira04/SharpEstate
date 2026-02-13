using System.ComponentModel.DataAnnotations;

namespace SharpEstate.Models
{
    public class Imovel
    {
        public int Id { get; set; }

        // --- DADOS PÚBLICOS ---
        [Required]
        public string Titulo { get; set; }

        [Required]
        public decimal Preco { get; set; }

        public string? Descricao { get; set; }
        public int Quartos { get; set; }
        public int CasasBanho { get; set; }
        public int Estacionamento { get; set; }
        public double AreaUtil { get; set; }
        public int AnoConstrucao { get; set; }
        public int NumeroFrentes { get; set; }

        // Localização (Pública)
        public string? Distrito { get; set; }
        public string? Concelho { get; set; }
        public string? Freguesia { get; set; }
        public string? Zona { get; set; }


        // --- DADOS PRIVADOS (SÓ O CONSULTOR VÊ) ---
        [Display(Name = "Morada Exata (Privado)")]
        public string? MoradaExata { get; set; }

        [Display(Name = "Número do Contrato")]
        public string? NumeroContrato { get; set; }

        [Display(Name = "Observações Internas")]
        public string? ObservacoesInternas { get; set; }

        public decimal? ValorComissao { get; set; }


        // --- CHAVES ESTRANGEIRAS (LIGAÇÃO AOS CATÁLOGOS E PESSOAS) ---
        public int? CategoriaImovelId { get; set; }
        public virtual CategoriaImovel? CategoriaImovel { get; set; }

        public int? TipoNegocioId { get; set; }
        public virtual TipoNegocio? TipoNegocio { get; set; }

        public int? EstadoImovelId { get; set; }
        public virtual EstadoImovel? EstadoImovel { get; set; }

        public int? StatusImovelId { get; set; }
        public virtual StatusImovel? StatusImovel { get; set; }

        public int? CertificadoEnergeticoId { get; set; }
        public virtual CertificadoEnergetico? CertificadoEnergetico { get; set; }

        public int? ConsultorId { get; set; }
        public virtual Consultor? Consultor { get; set; }


        // --- LISTAS DE LIGAÇÃO (Muitos-para-Muitos) ---
        public virtual ICollection<FotoImovel> Fotos { get; set; } = new List<FotoImovel>();
        public virtual ICollection<ImovelCaracteristica> Caracteristicas { get; set; } = new List<ImovelCaracteristica>();
        public virtual ICollection<ImovelProprietario> Proprietarios { get; set; } = new List<ImovelProprietario>();
        public virtual ICollection<ImovelFavorito> Favoritos { get; set; } = new List<ImovelFavorito>();
    }
}