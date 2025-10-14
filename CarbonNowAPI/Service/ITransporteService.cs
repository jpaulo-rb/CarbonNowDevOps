using CarbonNowAPI.Model;
using CarbonNowAPI.ViewModel;

namespace CarbonNowAPI.Service {
    public interface ITransporteService {
        Task<Paginacao<Transporte>> ListarTransportes(int pagina, int tamanho, string baseUrl);
        Task<IEnumerable<Transporte>> ListarTransportes();
        Task<Transporte> BuscarPorId(int id);
        Task<IEnumerable<Transporte>> BuscarPorData(DateTime inicio, DateTime fim);
        Task Adicionar(Transporte transporte);
        Task Editar(Transporte transporte);
        Task Deletar(int id);
        Task<IEnumerable<Transporte>> BuscarPorUsuario(int id);
    }
}
