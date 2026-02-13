namespace SharpEstate.Models
{
    public class Caracteristica
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string? Categoria { get; set; } // Ex: Segurança, Exterior
        public virtual ICollection<ImovelCaracteristica> ImoveisCaracteristicas { get; set; } = new List<ImovelCaracteristica>();
    }
}