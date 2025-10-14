using CarbonNowAPI.Model;

namespace CarbonNowAPI.Repository {
    public interface ITransporteRepository {
        Task<(IEnumerable<Transporte> Transportes, int Total)> ListarTransportes(int pagina, int tamanho);
        Task<IEnumerable<Transporte>> ListarTransportes();
        Task<Transporte?> BuscarPorId(int id);
        Task<IEnumerable<Transporte>> BuscarPorData(DateTime inicio, DateTime fim);
        void Adicionar(Transporte transporte);
        void Editar(Transporte transporte);
        void Deletar(Transporte transporte);
        Task SalvarContext();
        Task<bool> TransporteExiste(int id);
        Task<IEnumerable<Transporte>> BuscarPorUsuario(int id);
    }
}