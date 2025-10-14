using CarbonNowAPI.Model;
using CarbonNowAPI.ViewModel;
using CarbonNowAPI.ViewModel.Usuario;

namespace CarbonNowAPI.Service {
    public interface IUsuarioService {
        Task<string> Login(Usuario usuario);
        Task<Paginacao<Usuario>> ListarUsuarios(int pagina, int tamanho, string baseUrl);
        Task<IEnumerable<Usuario>> ListarUsuarios();
        Task<Usuario> BuscarPorId(int id);
        Task<Usuario> BuscarPorEmail(string email);
        Task Adicionar(Usuario usuario);
        Task Editar(Usuario usuario);
        Task Deletar(int id);
        Task CriarAdmin(Usuario usuario);
    }
}
