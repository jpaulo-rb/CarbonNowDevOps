using CarbonNowAPI.Exceptions;
using CarbonNowAPI.Model;
using CarbonNowAPI.Repository;
using CarbonNowAPI.ViewModel;

namespace CarbonNowAPI.Service {
    public class EletricidadeService : IEletricidadeService {

        private readonly IEletricidadeRepository _repository;
        private readonly IUsuarioRepository _usuarioRepository;

        public EletricidadeService(IEletricidadeRepository repository, IUsuarioRepository usuarioRepository) {
            _repository = repository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task Adicionar(Eletricidade eletricidade) {

            if (!await _usuarioRepository.UsuarioExiste(eletricidade.UsuarioId)) {
                throw new NotFoundException("UsuarioID", eletricidade.UsuarioId.ToString());
            };

            _repository.Adicionar(eletricidade);
            await _repository.SalvarContext();
        }

        public async Task<IEnumerable<Eletricidade>> BuscarPorData(DateTime inicio, DateTime fim) {
            return await _repository.BuscarPorData(inicio, fim);
        }

        public async Task<Eletricidade> BuscarPorId(int id) {
            var eletricidade = await _repository.BuscarPorId(id);

            if (eletricidade == null) {
                throw new NotFoundException("ID", id.ToString());
            };

            return eletricidade;
        }

        public async Task<IEnumerable<Eletricidade>> BuscarPorUsuario(int id) {

            if (!await _usuarioRepository.UsuarioExiste(id)) {
                throw new NotFoundException("UsuarioID", id.ToString());
            };

            return await _repository.BuscarPorUsuario(id);
        }

        public async Task Deletar(int id) {
            var eletricidade = await _repository.BuscarPorId(id);

            if (eletricidade == null) {
                throw new NotFoundException("ID", id.ToString());
            }

            _repository.Deletar(eletricidade);
            await _repository.SalvarContext();
        }

        public async Task Editar(Eletricidade eletricidade) {

            if (!await _usuarioRepository.UsuarioExiste(eletricidade.UsuarioId)) {
                throw new NotFoundException("UsuarioID", eletricidade.UsuarioId.ToString());
            };

            if (!await _repository.EletricidadeExiste(eletricidade.Id)) {
                throw new NotFoundException("ID", eletricidade.Id.ToString());
            }

            _repository.Editar(eletricidade);
            await _repository.SalvarContext();
        }

        public async Task<Paginacao<Eletricidade>> ListarEletricidades(int pagina, int tamanho, string baseUrl) {
            var (eletricidades, total) = await _repository.ListarEletricidades(pagina, tamanho);

            string BuildUrl(int p) => $"{baseUrl}?pagina={p}&tamanho={tamanho}";

            return new Paginacao<Eletricidade> {
                PaginaAtual = pagina,
                TamanhoPagina = tamanho,
                TotalItens = total,
                Itens = eletricidades,
                ProximaPaginaUrl = pagina * tamanho < total ? BuildUrl(pagina + 1) : null,
                PaginaAnteriorUrl = pagina > 1 ? BuildUrl(pagina - 1) : null
            };
        }

        public async Task<IEnumerable<Eletricidade>> ListarEletricidades() {
            return await _repository.ListarEletricidades();
        }
    }
}