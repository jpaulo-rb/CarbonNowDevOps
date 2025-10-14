using System.Security.Authentication;
using CarbonNowAPI.Exceptions;
using CarbonNowAPI.Model;
using CarbonNowAPI.Repository;
using CarbonNowAPI.Utils;
using CarbonNowAPI.ViewModel;
using CarbonNowAPI.ViewModel.Usuario;

namespace CarbonNowAPI.Service {
    public class UsuarioService : IUsuarioService {

        private readonly IUsuarioRepository _repository;
        private readonly TokenJWT _token;

        public UsuarioService(IUsuarioRepository repository, TokenJWT token) {
            _repository = repository;
            _token = token;
        }

        public async Task<string> Login(Usuario usuario) {

            var user = await _repository.BuscarPorEmail(usuario.Email);

            if (user == null) { 
                throw new InvalidCredentialException("Credenciais inválidas.");
            }

            var senhaValida = BCrypt.Net.BCrypt.Verify(usuario.Senha, user.Senha);
            if (senhaValida == false) {
                throw new InvalidCredentialException("Credenciais inválidas.");
            }

            return _token.GerarToken(user);
        }

        public async Task Adicionar(Usuario usuario) {
            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha, 12);

            if (await _repository.EmailExiste(usuario.Email)) {
                throw new ConflictException("E-mail", usuario.Email);
            }

            _repository.Adicionar(usuario);
            await _repository.SalvarContext();
        }

        public async Task<Usuario> BuscarPorEmail(string email) {
            var usuario = await _repository.BuscarPorEmail(email);

            if (usuario == null) {
                throw new NotFoundException("E-mail", email);
            }

            return usuario;
        }

        public async Task<Usuario> BuscarPorId(int id) {
            var usuario = await _repository.BuscarPorId(id);

            if (usuario == null) {
                throw new NotFoundException("ID", id.ToString());
            }

            return usuario;
        }

        public async Task Deletar(int id) {
            var usuario = await _repository.BuscarPorId(id);

            if (usuario == null) {
                throw new NotFoundException("ID", id.ToString());
            }

            _repository.Deletar(usuario);
            await _repository.SalvarContext();
        }

        public async Task Editar(Usuario usuario) {
            
            if (!await _repository.UsuarioExiste(usuario.Id)) {
                throw new NotFoundException("ID", usuario.Id.ToString());
            }

            var user = await _repository.BuscarPorEmail(usuario.Email);
            if (user != null && user.Id != usuario.Id) {
                throw new ConflictException("E-mail", usuario.Email);
            }

            _repository.Editar(usuario);
            await _repository.SalvarContext();
        }

        public async Task<Paginacao<Usuario>> ListarUsuarios(int pagina, int tamanho, string baseUrl) {
            var (usuarios, total) = await _repository.ListarUsuarios(pagina, tamanho);

            string BuildUrl(int p) => $"{baseUrl}?pagina={p}&tamanho={tamanho}";

            return new Paginacao<Usuario> {
                PaginaAtual = pagina,
                TamanhoPagina = tamanho,
                TotalItens = total,
                Itens = usuarios,
                ProximaPaginaUrl = pagina * tamanho < total ? BuildUrl(pagina + 1) : null,
                PaginaAnteriorUrl = pagina > 1 ? BuildUrl(pagina - 1) : null
            };
        }

        public async Task CriarAdmin(Usuario usuario) {

            if(await _repository.AdminExiste()) {
                throw new ConflictException("Admin", "Admin");
            }

            usuario.Regra = Regra.admin;

            await Adicionar(usuario);

        }

        public async Task<IEnumerable<Usuario>> ListarUsuarios() {
            return await _repository.ListarUsuarios();
        }
    }
}
