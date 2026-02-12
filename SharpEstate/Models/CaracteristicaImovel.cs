namespace SharpEstate.Models
{
    public class CaracteristicaImovel
    {
        public int Id { get; set; }

        // Ex: "Ar Condicionado", "Perto de Metro", "Água da Companhia"
        public string Nome { get; set; }

        // Ex: "Climatização", "Acessos", "Infraestruturas"
        public string Grupo { get; set; }

        public int ImovelId { get; set; }
        public virtual Imovel Imovel { get; set; }
    }
}