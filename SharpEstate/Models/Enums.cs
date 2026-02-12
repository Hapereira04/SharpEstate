using System.ComponentModel.DataAnnotations;

namespace SharpEstate.Models
{
    public enum TipoNegocio
    {
        Venda,
        Arrendamento,
        Trespass,
        Permuta
    }

    public enum EstadoImovel
    {
        Novo,
        Usado,
        ParaRecuperar,
        EmConstrucao,
        Ruina
    }

    public enum CertificadoEnergetico
    {
        [Display(Name = "A+")] AMais,
        A,
        [Display(Name = "B+")] BMais,
        B,
        [Display(Name = "B-")] BMenos,
        C,
        D,
        E,
        F,
        Isento
    }

    public enum StatusSistema
    {
        Disponivel,
        Reservado,
        Vendido,
        Inativo
    }

    public enum NivelUrgencia
    {
        Baixa,
        Media,
        Alta,
        Imediata
    }
}