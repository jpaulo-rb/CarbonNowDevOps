using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using CarbonNowAPI.Model;
using CarbonNowAPI.Service;
using CarbonNowAPI.ViewModel;
using CarbonNowAPI.ViewModel.Eletricidade;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarbonNowAPI.Controller
{

    [ApiVersion(1.0, Deprecated = true)]
    [ApiVersion(1.1)]
    [ApiController]
    [Route("api/v{v:apiVersion}/[controller]")]
    [Authorize]
    public class EletricidadeController : ControllerBase
    {

        private readonly IEletricidadeService _eletricidadeService;
        private readonly IMapper _mapper;

        public EletricidadeController(IEletricidadeService eletricidadeService, IMapper mapper)
        {
            _mapper = mapper;
            _eletricidadeService = eletricidadeService;
        }

        [MapToApiVersion(1.1)]
        [Authorize(Policy = "Normal")]
        [HttpGet]
        public async Task<ActionResult<Paginacao<EletricidadeViewModel>>> ListarTodosEletricidades([FromQuery] int pagina = 1, [FromQuery] int tamanho = 20)
        {

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            var resultado = await _eletricidadeService.ListarEletricidades(pagina, tamanho, baseUrl);

            var eletricidades = new Paginacao<EletricidadeViewModel>
            {
                PaginaAtual = resultado.PaginaAtual,
                TamanhoPagina = resultado.TamanhoPagina,
                TotalItens = resultado.TotalItens,
                Itens = _mapper.Map<List<EletricidadeViewModel>>(resultado.Itens),
                ProximaPaginaUrl = resultado.ProximaPaginaUrl,
                PaginaAnteriorUrl = resultado.PaginaAnteriorUrl
            };

            return Ok(eletricidades);
        }

        [MapToApiVersion(1.0)]
        [Authorize(Policy = "Normal")]
        [HttpGet]
        public async Task<ActionResult<Paginacao<EletricidadeViewModel>>> ListarTodosEletricidades()
        {
            return Ok(_mapper.Map<List<EletricidadeViewModel>>(await _eletricidadeService.ListarEletricidades()));
        }

        [Authorize(Policy = "Normal")]
        [HttpGet("{id}")]
        public async Task<ActionResult<EletricidadeViewModel>> BuscarEletricidadePorId(int id)
        {

            var eletricidade = await _eletricidadeService.BuscarPorId(id);
            return Ok(_mapper.Map<EletricidadeViewModel>(eletricidade));
        }

        [Authorize(Policy = "Normal")]
        [HttpGet("data/")]
        public async Task<ActionResult<EletricidadeViewModel>> BuscarEletricidadePorData([FromQuery] DateOnly inicio, DateOnly fim)
        {

            if (fim < inicio)
            {
                return BadRequest("Data final menor que a data inicial.");
            }

            var eletricidade = await _eletricidadeService.BuscarPorData(inicio.ToDateTime(TimeOnly.MinValue), fim.ToDateTime(TimeOnly.MinValue));
            return Ok(_mapper.Map<List<EletricidadeViewModel>>(eletricidade));
        }

        [Authorize(Policy = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarEletricidade(int id, EletricidadeViewModel eletricidade)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != eletricidade.Id)
            {
                return BadRequest("ID da eletricidade na URL difere do ID enviado.");
            }

            var electricity = _mapper.Map<Eletricidade>(eletricidade);

            await _eletricidadeService.Editar(electricity);
            return NoContent();

        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<ActionResult<EletricidadeViewModel>> AdicionarEletricidade(EletricidadeCadastroViewModel eletricidade)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int usuarioId))
            {
                return Unauthorized("Usuário não autenticado.");
            }

            var electricity = _mapper.Map<Eletricidade>(eletricidade);

            electricity.UsuarioId = usuarioId;

            await _eletricidadeService.Adicionar(electricity);
            return CreatedAtAction("BuscarEletricidadePorId", new { id = electricity.Id }, _mapper.Map<EletricidadeViewModel>(electricity));
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarEletricidade(int id)
        {

            await _eletricidadeService.Deletar(id);
            return NoContent();

        }


        [Authorize(Policy = "Normal")]
        [HttpGet("usuario/{id}")]
        public async Task<IActionResult> BuscarEletricidadePorUsuario(int id)
        {
            return Ok(_mapper.Map<List<EletricidadeViewModel>>(await _eletricidadeService.BuscarPorUsuario(id)));
        }
    }
}
