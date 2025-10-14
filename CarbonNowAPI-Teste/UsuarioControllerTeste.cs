using Xunit;
using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using CarbonNowAPI.Controller;
using CarbonNowAPI.Service;
using CarbonNowAPI.Model;
using CarbonNowAPI.ViewModel;
using CarbonNowAPI.ViewModel.Usuario;
using System.Threading.Tasks;
using System.Collections.Generic;

public class UsuarioControllerTeste {
    private readonly Mock<IUsuarioService> _mockService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly UsuarioController _controller;

    public UsuarioControllerTeste() {
        _mockService = new Mock<IUsuarioService>();
        _mockMapper = new Mock<IMapper>();
        _controller = new UsuarioController(_mockService.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task ListarTodosUsuarios_Versao11_DeveRetornarOkComPaginacao() {
        // Arrange
        var paginacaoModel = new Paginacao<Usuario> {
            PaginaAtual = 1,
            TamanhoPagina = 10,
            TotalItens = 2,
            Itens = new List<Usuario> { new Usuario { Id = 1 }, new Usuario { Id = 2 } },
            ProximaPaginaUrl = null,
            PaginaAnteriorUrl = null
        };
        var paginacaoVm = new Paginacao<UsuarioExibicaoViewModel> {
            PaginaAtual = 1,
            TamanhoPagina = 10,
            TotalItens = 2,
            Itens = new List<UsuarioExibicaoViewModel> {
                new UsuarioExibicaoViewModel { Id = 1 },
                new UsuarioExibicaoViewModel { Id = 2 }
            },
            ProximaPaginaUrl = null,
            PaginaAnteriorUrl = null
        };

        _mockService.Setup(s => s.ListarUsuarios(1, 20, It.IsAny<string>())).ReturnsAsync(paginacaoModel);
        _mockMapper
            .Setup(m => m.Map<List<UsuarioExibicaoViewModel>>(It.IsAny<IEnumerable<Usuario>>()))
            .Returns(paginacaoVm.Itens.ToList());

        // Simula URL base para montar a response
        _controller.ControllerContext = new ControllerContext {
            HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext()
        };
        _controller.ControllerContext.HttpContext.Request.Scheme = "https";
        _controller.ControllerContext.HttpContext.Request.Host = new Microsoft.AspNetCore.Http.HostString("localhost");
        _controller.ControllerContext.HttpContext.Request.Path = "/api/v1.1/usuario";

        // Act
        var result = await _controller.ListarTodosUsuarios(1, 20);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var paginacaoRetornada = Assert.IsType<Paginacao<UsuarioExibicaoViewModel>>(okResult.Value);
        Assert.Equal(2, paginacaoRetornada.TotalItens);
        Assert.Equal(1, paginacaoRetornada.PaginaAtual);
    }

    [Fact]
    public async Task BuscarUsuarioPorId_DeveRetornarOkComUsuario() {
        // Arrange
        var id = 1;
        var usuarioModel = new Usuario { Id = id };
        var usuarioVm = new UsuarioExibicaoViewModel { Id = id };

        _mockService.Setup(s => s.BuscarPorId(id)).ReturnsAsync(usuarioModel);
        _mockMapper.Setup(m => m.Map<UsuarioExibicaoViewModel>(usuarioModel)).Returns(usuarioVm);

        // Act
        var result = await _controller.BuscarUsuarioPorId(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var vm = Assert.IsType<UsuarioExibicaoViewModel>(okResult.Value);
        Assert.Equal(id, vm.Id);
    }
}
