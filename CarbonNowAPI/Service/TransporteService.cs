using CarbonNowAPI.Exceptions;
using CarbonNowAPI.Model;
using CarbonNowAPI.Repository;
using CarbonNowAPI.ViewModel;

namespace CarbonNowAPI.Service {
    public class TransporteService : ITransporteService {

        private readonly ITransporteRepository _repository;
        private readonly IUsuarioRepository _usuarioRepository;

        public TransporteService(ITransporteRepository repository, IUsuarioRepository usuarioRepository) {
            _repository = repository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task Adicionar(Transporte transporte) {

            if (!await _usuarioRepository.UsuarioExiste(transporte.UsuarioId)) {
                throw new NotFoundException("UsuarioID", transporte.UsuarioId.ToString());
            };

            _repository.Adicionar(transporte);
            await _repository.SalvarContext();
        }

        public async Task<IEnumerable<Transporte>> BuscarPorData(DateTime inicio, DateTime fim) {
            return await _repository.BuscarPorData(inicio, fim);
        }

        public async Task<Transporte> BuscarPorId(int id) {
            var transporte = await _repository.BuscarPorId(id);

            if (transporte == null) {
                throw new NotFoundException("ID", id.ToString());
            };

            return transporte;
        }

        public async Task<IEnumerable<Transporte>> BuscarPorUsuario(int id) {
            if (!await _usuarioRepository.UsuarioExiste(id)) {
                throw new NotFoundException("ID", id.ToString());
            }

            return await _repository.BuscarPorUsuario(id);
        }

        public async Task Deletar(int id) {
            var transporte = await _repository.BuscarPorId(id);

            if (transporte == null) {
                throw new NotFoundException("ID", id.ToString());
            }

            _repository.Deletar(transporte);
            await _repository.SalvarContext();
        }

        public async Task Editar(Transporte transporte) {

            if (!await _usuarioRepository.UsuarioExiste(transporte.UsuarioId)) {
                throw new NotFoundException("UsuarioID", transporte.UsuarioId.ToString());
            };

            if (!await _repository.TransporteExiste(transporte.Id)) {
                throw new NotFoundException("ID", transporte.Id.ToString());
            }

            _repository.Editar(transporte);
            await _repository.SalvarContext();
        }

        public async Task<Paginacao<Transporte>> ListarTransportes(int pagina, int tamanho, string baseUrl) {
            var (transportes, total) = await _repository.ListarTransportes(pagina, tamanho);

            string BuildUrl(int p) => $"{baseUrl}?pagina={p}&tamanho={tamanho}";

            return new Paginacao<Transporte> {
                PaginaAtual = pagina,
                TamanhoPagina = tamanho,
                TotalItens = total,
                Itens = transportes,
                ProximaPaginaUrl = pagina * tamanho < total ? BuildUrl(pagina + 1) : null,
                PaginaAnteriorUrl = pagina > 1 ? BuildUrl(pagina - 1) : null
            };
        }

        public async Task<IEnumerable<Transporte>> ListarTransportes() {
            return await _repository.ListarTransportes();
        }
    }
}