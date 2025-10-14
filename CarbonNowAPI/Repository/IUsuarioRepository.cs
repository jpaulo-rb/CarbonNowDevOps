using CarbonNowAPI.Model;
using CarbonNowAPI.ViewModel;

namespace CarbonNowAPI.Repository {
    public interface IUsuarioRepository {
        Task<Usuario?> LoginUsuario(Usuario usuario);
        Task<(IEnumerable<Usuario> Usuarios, int Total)> ListarUsuarios(int pagina, int tamanho);
        Task<IEnumerable<Usuario>> ListarUsuarios();
        Task<Usuario?> BuscarPorId(int id);
        Task<Usuario?> BuscarPorEmail(string email);
        void Adicionar(Usuario usuario);
        void Editar(Usuario usuario);
        void Deletar(Usuario usuario);
        Task SalvarContext();
        Task<bool> UsuarioExiste(int id);
        Task<bool> EmailExiste(string email);
        Task<bool> AdminExiste();
    }
}
