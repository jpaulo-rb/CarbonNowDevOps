using CarbonNowAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarbonNowAPI.Data.TableConfig
{
    public class UsuarioConfig : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> entity)
        {

            entity.ToTable("Usuario");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasDefaultValueSql("SEQ_ID_USUARIO.NEXTVAL");
            // .HasDefaultValueSql("nextval('\"SEQ_ID_USUARIO\"')");

            entity.Property(e => e.Nome)
                .HasMaxLength(64);

            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Email)
                .HasMaxLength(128);

            entity.Property(e => e.Senha)
                .HasMaxLength(60);

            entity.Property(e => e.Regra)
                .HasConversion<string>()
                .HasMaxLength(10);
        }
    }
}
