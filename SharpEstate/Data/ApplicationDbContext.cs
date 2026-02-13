using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharpEstate.Models;

namespace SharpEstate.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Imovel> Imoveis { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<ImovelProprietario> ImoveisProprietarios { get; set; }
        public DbSet<CaracteristicaImovel> Caracteristicas { get; set; }
        public DbSet<FotoImovel> Fotos { get; set; }
        public DbSet<InteresseCompra> InteressesCompra { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuração da Relação Muitos-para-Muitos (Imovel <-> Proprietarios)
            builder.Entity<ImovelProprietario>()
                .HasKey(ip => new { ip.ImovelId, ip.ClienteId });

            builder.Entity<ImovelProprietario>()
                .HasOne(ip => ip.Imovel)
                .WithMany(i => i.Proprietarios)
                .HasForeignKey(ip => ip.ImovelId);

            builder.Entity<ImovelProprietario>()
                .HasOne(ip => ip.Cliente)
                .WithMany(c => c.ImoveisProprietario)
                .HasForeignKey(ip => ip.ClienteId);

            // Garante precisão monetária
            builder.Entity<Imovel>()
                .Property(p => p.Preco)
                .HasColumnType("decimal(18,2)");

            builder.Entity<InteresseCompra>()
                .Property(p => p.PrecoMaximo)
                .HasColumnType("decimal(18,2)");

            builder.Entity<ImovelCaracteristica>()
                .HasKey(ic => new { ic.ImovelId, ic.CaracteristicaId });

            // Relação Imovel -> ImovelCaracteristica (Se apagar o imóvel, apaga a ligação)
            builder.Entity<ImovelCaracteristica>()
                .HasOne(ic => ic.Imovel)
                .WithMany(i => i.Caracteristicas)
                .HasForeignKey(ic => ic.ImovelId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relação Caracteristica -> ImovelCaracteristica (NÃO PODE APAGAR SE ESTIVER EM USO)
            builder.Entity<ImovelCaracteristica>()
                .HasOne(ic => ic.Caracteristica)
                .WithMany(c => c.ImoveisCaracteristicas)
                .HasForeignKey(ic => ic.CaracteristicaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}