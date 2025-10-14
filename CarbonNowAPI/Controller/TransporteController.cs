using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using CarbonNowAPI.Model;
using CarbonNowAPI.Service;
using CarbonNowAPI.ViewModel;
using CarbonNowAPI.ViewModel.Transporte;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarbonNowAPI.Controller {

    [ApiVersion(1.0, Deprecated = true)]
    [ApiVersion(1.1)]
    [ApiController]
    [Route("api/v{v:apiVersion}/[controller]")]
    [Authorize]
    public class TransporteController : ControllerBase {

        private readonly ITransporteService _transporteService;
        private readonly IMapper _mapper;

        public TransporteController(ITransporteService transporteService, IMapper mapper) {
            _mapper = mapper;
            _transporteService = transporteService;
        }

        [MapToApiVersion(1.1)]
        [Authorize(Policy = "Normal")]
        [HttpGet]
        public async Task<ActionResult<Paginacao<TransporteViewModel>>> ListarTodosTransportes([FromQuery] int pagina = 1, [FromQuery] int tamanho = 20) {

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            var resultado = await _transporteService.ListarTransportes(pagina, tamanho, baseUrl);

            var transportes = new Paginacao<TransporteViewModel> {
                PaginaAtual = resultado.PaginaAtual,
                TamanhoPagina = resultado.TamanhoPagina,
                TotalItens = resultado.TotalItens,
                Itens = _mapper.Map<List<TransporteViewModel>>(resultado.Itens),
                ProximaPaginaUrl = resultado.ProximaPaginaUrl,
                PaginaAnteriorUrl = resultado.PaginaAnteriorUrl
            };

            return Ok(transportes);
        }

        [MapToApiVersion(1.0)]
        [Authorize(Policy = "Normal")]
        [HttpGet]
        public async Task<ActionResult<Paginacao<TransporteViewModel>>> ListarTodosTransportes() {
            return Ok(_mapper.Map<List<TransporteViewModel>>(await _transporteService.ListarTransportes()));
        }

        [Authorize(Policy = "Normal")]
        [HttpGet("{id}")]
        public async Task<ActionResult<TransporteViewModel>> BuscarTransportePorId(int id) {

            var transporte = await _transporteService.BuscarPorId(id);
            return Ok(_mapper.Map<TransporteViewModel>(transporte));
        }

        [Authorize(Policy = "Normal")]
        [HttpGet("data/")]
        public async Task<ActionResult<TransporteViewModel>> BuscarTransportePorData([FromQuery] DateOnly inicio, DateOnly fim) {

            if (fim < inicio) {
                return BadRequest("Data final menor que a data inicial.");
            }

            var transporte = await _transporteService.BuscarPorData(inicio.ToDateTime(TimeOnly.MinValue), fim.ToDateTime(TimeOnly.MinValue));
            return Ok(_mapper.Map<List<TransporteViewModel>>(transporte));
        }

        [Authorize(Policy = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarTransporte(int id, TransporteViewModel transporte) {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (id != transporte.Id) {
                return BadRequest("ID da transporte na URL difere do ID enviado.");
            }

            var electricity = _mapper.Map<Transporte>(transporte);

            await _transporteService.Editar(electricity);
            return NoContent();

        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<ActionResult<TransporteViewModel>> AdicionarTransporte(TransporteCadastroViewModel transporte) {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int usuarioId)) {
                return Unauthorized("Usuário não autenticado.");
            }

            var electricity = _mapper.Map<Transporte>(transporte);

            electricity.UsuarioId = usuarioId;

            await _transporteService.Adicionar(electricity);
            return CreatedAtAction("BuscarTransportePorId", new { id = electricity.Id }, _mapper.Map<TransporteViewModel>(electricity));
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarTransporte(int id) {

            await _transporteService.Deletar(id);
            return NoContent();

        }

        [Authorize(Policy = "Normal")]
        [HttpGet("usuario/{id}")]
        public async Task<IActionResult> BuscarEletricidadePorUsuario(int id) {
            return Ok(_mapper.Map<List<TransporteViewModel>>(await _transporteService.BuscarPorUsuario(id)));
        }
    }
}
