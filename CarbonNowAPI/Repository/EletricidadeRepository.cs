using CarbonNowAPI.Data;
using CarbonNowAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace CarbonNowAPI.Repository
{
    public class EletricidadeRepository : IEletricidadeRepository
    {

        private readonly DatabaseConnection _context;

        public EletricidadeRepository(DatabaseConnection context)
        {
            _context = context;
        }

        public void Adicionar(Eletricidade eletricidade)
        {
            _context.Add(eletricidade);
        }

        public async Task<IEnumerable<Eletricidade>> BuscarPorData(DateTime inicio, DateTime fim)
        {
            return await _context.Eletricidade
                .AsNoTracking()
                .OrderBy(e => e.DataEstimacao).ThenBy(e => e.Id)
                .Where(e => e.DataEstimacao >= inicio && e.DataEstimacao <= fim)
                .ToListAsync();
        }

        public async Task<Eletricidade?> BuscarPorId(int id)
        {
            return await _context.Eletricidade.FindAsync(id);
        }

        public void Deletar(Eletricidade eletricidade)
        {
            _context.Remove(eletricidade);
        }

        public void Editar(Eletricidade eletricidade)
        {
            _context.Eletricidade.Update(eletricidade);
        }

        public async Task<(IEnumerable<Eletricidade> Eletricidades, int Total)> ListarEletricidades(int pagina, int tamanho)
        {
            var query = _context.Eletricidade.AsNoTracking().OrderBy(e => e.Id);

            var total = await query.CountAsync();

            var eletricidades = await query
                .Skip((pagina - 1) * tamanho)
                .Take(tamanho)
                .ToListAsync();

            return (eletricidades, total);
        }

        public async Task<IEnumerable<Eletricidade>> ListarEletricidades()
        {
            return await _context.Eletricidade.AsNoTracking().OrderBy(e => e.Id).ToListAsync();
        }

        public async Task SalvarContext()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EletricidadeExiste(int id)
        {
            return await _context.Eletricidade
                .Where(e => e.Id == id)
                .Select(e => 1)
                .FirstOrDefaultAsync() == 1;
        }

        public async Task<IEnumerable<Eletricidade>> BuscarPorUsuario(int id)
        {
            return await _context.Eletricidade
                .AsNoTracking()
                .Where(e => e.UsuarioId == id)
                .OrderBy(e => e.Id)
                .ToListAsync();
        }
    }
}
