using CarbonNowAPI.Data;
using CarbonNowAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace CarbonNowAPI.Repository
{
    public class TransporteRepository : ITransporteRepository
    {

        private readonly DatabaseConnection _context;

        public TransporteRepository(DatabaseConnection context)
        {
            _context = context;
        }

        public void Adicionar(Transporte transporte)
        {
            _context.Add(transporte);
        }

        public async Task<IEnumerable<Transporte>> BuscarPorData(DateTime inicio, DateTime fim)
        {
            return await _context.Transporte
                .AsNoTracking()
                .OrderBy(e => e.DataEstimacao).ThenBy(e => e.Id)
                .Where(e => e.DataEstimacao >= inicio && e.DataEstimacao <= fim)
                .ToListAsync();
        }

        public async Task<Transporte?> BuscarPorId(int id)
        {
            return await _context.Transporte.FindAsync(id);
        }

        public async Task<IEnumerable<Transporte>> BuscarPorUsuario(int id)
        {
            return await _context.Transporte
                .AsNoTracking()
                .Where(e => e.UsuarioId == id)
                .OrderBy(e => e.Id)
                .ToListAsync();
        }

        public void Deletar(Transporte transporte)
        {
            _context.Remove(transporte);
        }

        public void Editar(Transporte transporte)
        {
            _context.Transporte.Update(transporte);
        }

        public async Task<(IEnumerable<Transporte> Transportes, int Total)> ListarTransportes(int pagina, int tamanho)
        {
            var query = _context.Transporte.AsNoTracking().OrderBy(e => e.Id);

            var total = await query.CountAsync();

            var transportes = await query
                .Skip((pagina - 1) * tamanho)
                .Take(tamanho)
                .ToListAsync();

            return (transportes, total);
        }

        public async Task<IEnumerable<Transporte>> ListarTransportes()
        {
            return await _context.Transporte.AsNoTracking().OrderBy(e => e.Id).ToListAsync();
        }

        public async Task SalvarContext()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> TransporteExiste(int id)
        {
            return await _context.Transporte
                .Where(e => e.Id == id)
                .Select(e => 1)
                .FirstOrDefaultAsync() == 1;
        }
    }
}
