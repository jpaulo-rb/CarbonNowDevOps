using CarbonNowAPI.Model;

namespace CarbonNowAPI.Repository {
    public interface IEletricidadeRepository {
        Task<(IEnumerable<Eletricidade> Eletricidades, int Total)> ListarEletricidades(int pagina, int tamanho);
        Task<IEnumerable<Eletricidade>> ListarEletricidades();
        Task<Eletricidade?> BuscarPorId(int id);
        Task<IEnumerable<Eletricidade>> BuscarPorData(DateTime inicio, DateTime fim);
        void Adicionar(Eletricidade eletricidade);
        void Editar(Eletricidade eletricidade);
        void Deletar(Eletricidade eletricidade);
        Task SalvarContext();
        Task<bool> EletricidadeExiste(int id);
        Task<IEnumerable<Eletricidade>> BuscarPorUsuario(int id);
    }
}