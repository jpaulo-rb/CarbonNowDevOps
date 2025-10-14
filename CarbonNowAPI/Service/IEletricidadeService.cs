using CarbonNowAPI.Model;
using CarbonNowAPI.ViewModel;

namespace CarbonNowAPI.Service {
    public interface IEletricidadeService {
        Task<Paginacao<Eletricidade>> ListarEletricidades(int pagina, int tamanho, string baseUrl);
        Task<IEnumerable<Eletricidade>> ListarEletricidades();
        Task<Eletricidade> BuscarPorId(int id);
        Task<IEnumerable<Eletricidade>> BuscarPorData(DateTime inicio, DateTime fim);
        Task Adicionar(Eletricidade eletricidade);
        Task Editar(Eletricidade eletricidade);
        Task Deletar(int id);
        Task<IEnumerable<Eletricidade>> BuscarPorUsuario(int id);
    }
}
