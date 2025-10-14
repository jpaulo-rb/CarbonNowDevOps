using CarbonNowAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarbonNowAPI.Data.TableConfig
{
    public class TransporteConfig : IEntityTypeConfiguration<Transporte>
    {
        public void Configure(EntityTypeBuilder<Transporte> entity)
        {

            entity.ToTable("Transporte");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasDefaultValueSql("SEQ_ID_TRANSPORTE.NEXTVAL");
            // .HasDefaultValueSql("nextval('\"SEQ_ID_TRANSPORTE\"')");

            entity.HasOne(e => e.Usuario)
                .WithMany(u => u.Transporte)
                .HasForeignKey(e => e.UsuarioId);

            entity.Property(e => e.MetodoTransporte)
                .HasConversion<string>()
                .HasMaxLength(5);

            entity.Property(e => e.DataEstimacao)
                .HasDefaultValueSql("TRUNC(SYSDATE)");
            // .HasDefaultValueSql("CURRENT_DATE");

            entity.Property(e => e.ValorPesoKg).HasColumnType("NUMBER(18,2)");
            entity.Property(e => e.ValorDistanciaKm).HasColumnType("NUMBER(18,2)");
            entity.Property(e => e.CarbonoKg).HasColumnType("NUMBER(18,2)");

        }
    }
}