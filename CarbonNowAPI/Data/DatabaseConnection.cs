using CarbonNowAPI.Data.TableConfig;
using CarbonNowAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace CarbonNowAPI.Data
{
    public class DatabaseConnection : DbContext
    {

        private readonly IConfiguration _configuration;

        public DatabaseConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("Oracle");
            ArgumentNullException.ThrowIfNullOrWhiteSpace(connectionString, "ConnectionString");
            optionsBuilder.UseOracle(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasSequence<int>("SEQ_ID_ELETRICIDADE")
                .StartsAt(1)
                .IncrementsBy(1);

            modelBuilder.HasSequence<int>("SEQ_ID_TRANSPORTE")
                .StartsAt(1)
                .IncrementsBy(1);

            modelBuilder.HasSequence<int>("SEQ_ID_USUARIO")
                .StartsAt(1)
                .IncrementsBy(1);

            modelBuilder.ApplyConfiguration(new UsuarioConfig());
            modelBuilder.ApplyConfiguration(new EletricidadeConfig());
            modelBuilder.ApplyConfiguration(new TransporteConfig());

        }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Eletricidade> Eletricidade { get; set; }
        public DbSet<Transporte> Transporte { get; set; }

    }
}
