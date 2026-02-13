namespace SharpEstate.Models
{
    public class FotoImovel
    {
        public int Id { get; set; }
        public byte[] DadosImagem { get; set; }
        public string ContentType { get; set; }
        public bool IsCapa { get; set; }

        public int ImovelId { get; set; }
        public virtual Imovel Imovel { get; set; }
    }
}