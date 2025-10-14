using CarbonNowAPI.Data;
using CarbonNowAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace CarbonNowAPI.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {

        private readonly DatabaseConnection _context;

        public UsuarioRepository(DatabaseConnection context)
        {
            _context = context;
        }

        public async Task<Usuario?> LoginUsuario(Usuario usuario)
        {
            return await _context.Usuario.FirstOrDefaultAsync(u => u.Email == usuario.Email && u.Senha == usuario.Senha);
        }

        public async Task<(IEnumerable<Usuario> Usuarios, int Total)> ListarUsuarios(int pagina, int tamanho)
        {

            var query = _context.Usuario.AsNoTracking().OrderBy(u => u.Id);

            var total = await query.CountAsync();

            var usuarios = await query
                .Skip((pagina - 1) * tamanho)
                .Take(tamanho)
                .ToListAsync();

            return (usuarios, total);
        }

        public void Adicionar(Usuario usuario)
        {
            _context.Usuario.Add(usuario);
        }

        public async Task<Usuario?> BuscarPorId(int id)
        {
            return await _context.Usuario.FindAsync(id);
        }

        public async Task<Usuario?> BuscarPorEmail(string email)
        {
            return await _context.Usuario.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public void Deletar(Usuario usuario)
        {
            _context.Usuario.Remove(usuario);
        }

        public void Editar(Usuario usuario)
        {
            _context.Usuario.Update(usuario);
        }

        public async Task SalvarContext()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UsuarioExiste(int id)
        {
            return await _context.Usuario
                .Where(u => u.Id == id)
                .Select(u => 1)
                .FirstOrDefaultAsync() == 1;
        }

        public async Task<bool> EmailExiste(string email)
        {
            return await _context.Usuario
                .Where(u => u.Email.ToLower() == email.ToLower())
                .Select(u => 1)
                .FirstOrDefaultAsync() == 1;
        }

        public async Task<bool> AdminExiste()
        {
            return await _context.Usuario
                .Where(u => u.Regra == Regra.admin)
                .Select(u => 1)
                .FirstOrDefaultAsync() == 1;
        }

        public async Task<IEnumerable<Usuario>> ListarUsuarios()
        {
            return await _context.Usuario.AsNoTracking().OrderBy(u => u.Id).ToListAsync();
        }
    }
}
