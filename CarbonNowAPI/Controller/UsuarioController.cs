using Asp.Versioning;
using AutoMapper;
using CarbonNowAPI.Model;
using CarbonNowAPI.Service;
using CarbonNowAPI.ViewModel;
using CarbonNowAPI.ViewModel.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarbonNowAPI.Controller {

    [ApiVersion(1.0, Deprecated = true)]
    [ApiVersion(1.1)]
    [ApiController]
    [Route("api/v{v:apiVersion}/[controller]")]
    [Authorize]
    public class UsuarioController : ControllerBase {

        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;

        public UsuarioController(IUsuarioService usuarioService, IMapper mapper) {
            _mapper = mapper;
            _usuarioService = usuarioService;
        }

        [MapToApiVersion(1.1)]
        [Authorize(Policy = "Normal")]
        [HttpGet]
        public async Task<ActionResult<Paginacao<UsuarioExibicaoViewModel>>> ListarTodosUsuarios([FromQuery] int pagina = 1, [FromQuery] int tamanho = 20) {

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            var resultado = await _usuarioService.ListarUsuarios(pagina, tamanho, baseUrl);

            var usuarios = new Paginacao<UsuarioExibicaoViewModel> {
                PaginaAtual = resultado.PaginaAtual,
                TamanhoPagina = resultado.TamanhoPagina,
                TotalItens = resultado.TotalItens,
                Itens = _mapper.Map<List<UsuarioExibicaoViewModel>>(resultado.Itens),
                ProximaPaginaUrl = resultado.ProximaPaginaUrl,
                PaginaAnteriorUrl = resultado.PaginaAnteriorUrl
            };

            return Ok(usuarios);
        }

        [MapToApiVersion(1.0)]
        [Authorize(Policy = "Normal")]
        [HttpGet]
        public async Task<ActionResult<Paginacao<UsuarioExibicaoViewModel>>> ListarTodosUsuarios() {
            return Ok(_mapper.Map<List<UsuarioExibicaoViewModel>>(await _usuarioService.ListarUsuarios()));
        }

        [Authorize(Policy = "Normal")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioExibicaoViewModel>> BuscarUsuarioPorId(int id) {

            var usuario = await _usuarioService.BuscarPorId(id);
            return Ok(_mapper.Map<UsuarioExibicaoViewModel>(usuario));
        }

        [Authorize(Policy = "Normal")]
        [HttpGet("email/{email}")]
        public async Task<ActionResult<UsuarioExibicaoViewModel>> BuscarUsuarioPorEmail(string email) {

            var usuario = await _usuarioService.BuscarPorEmail(email);
            return Ok(_mapper.Map<UsuarioExibicaoViewModel>(usuario));
        }

        [Authorize(Policy = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarUsuario(int id, UsuarioAlteracaoViewModel usuario) {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (id != usuario.Id) {
                return BadRequest("ID do usuário na URL difere do ID enviado.");
            }

            var user = _mapper.Map<Usuario>(usuario);

            await _usuarioService.Editar(user);
            return NoContent();

        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<ActionResult<UsuarioExibicaoViewModel>> AdicionarUsuario(UsuarioCadastroViewModel usuario) {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var user = _mapper.Map<Usuario>(usuario);

            await _usuarioService.Adicionar(user);
            return CreatedAtAction("BuscarUsuarioPorId", new { id = user.Id }, _mapper.Map<UsuarioExibicaoViewModel>(user));
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarUsuario(int id) {

            await _usuarioService.Deletar(id);
            return NoContent();

        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginUsuario(UsuarioLoginViewModel usuario) {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            return Ok(await _usuarioService.Login(_mapper.Map<Usuario>(usuario)));
        }

        [HttpPost("criaradmin")]
        [AllowAnonymous]
        public async Task<ActionResult<UsuarioExibicaoViewModel>> CriarAdmin(UsuarioCadastroViewModel usuario) {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var user = _mapper.Map<Usuario>(usuario);

            await _usuarioService.CriarAdmin(user);

            return CreatedAtAction("BuscarUsuarioPorId", new { id = user.Id }, _mapper.Map<UsuarioExibicaoViewModel>(user));
        }
    }
}
