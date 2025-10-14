using System.Security.Claims;
using AutoMapper;
using CarbonNowAPI.Controller;
using CarbonNowAPI.Model;
using CarbonNowAPI.Service;
using CarbonNowAPI.ViewModel.Eletricidade;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

public class EletricidadeControllerTests
{
    private readonly Mock<IEletricidadeService> _mockService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly EletricidadeController _controller;

    public EletricidadeControllerTests()
    {
        _mockService = new Mock<IEletricidadeService>();
        _mockMapper = new Mock<IMapper>();
        _controller = new EletricidadeController(_mockService.Object, _mockMapper.Object);

        // Mock User Claims para simular usuário autenticado
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "123")
        }, "mock"));

        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = user }
        };
    }

    [Fact]
    public async Task AtualizarEletricidade_DeveRetornarNoContent_QuandoOk()
    {
        // Arrange
        var id = 1;
        var vm = new EletricidadeViewModel { Id = id };
        var model = new Eletricidade { Id = id, UsuarioId = 123 };

        _mockMapper.Setup(m => m.Map<Eletricidade>(vm)).Returns(model);
        _mockService.Setup(s => s.Editar(model)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AtualizarEletricidade(id, vm);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task AdicionarEletricidade_DeveRetornarCreatedAtAction_QuandoOk()
    {
        // Arrange
        var cadastroVm = new EletricidadeCadastroViewModel();
        var model = new Eletricidade { Id = 1, UsuarioId = 123 };
        var vm = new EletricidadeViewModel { Id = 1 };

        _mockMapper.Setup(m => m.Map<Eletricidade>(cadastroVm)).Returns(model);
        _mockService.Setup(s => s.Adicionar(model)).Returns(Task.CompletedTask);
        _mockMapper.Setup(m => m.Map<EletricidadeViewModel>(model)).Returns(vm);

        // Act
        var result = await _controller.AdicionarEletricidade(cadastroVm);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal("BuscarEletricidadePorId", createdResult.ActionName);
        Assert.Equal(vm, createdResult.Value);
    }

    [Fact]
    public async Task BuscarEletricidadePorId_DeveRetornarOkComVm()
    {
        // Arrange
        var id = 1;
        var model = new Eletricidade { Id = id };
        var vm = new EletricidadeViewModel { Id = id };

        _mockService.Setup(s => s.BuscarPorId(id)).ReturnsAsync(model);
        _mockMapper.Setup(m => m.Map<EletricidadeViewModel>(model)).Returns(vm);

        // Act
        var result = await _controller.BuscarEletricidadePorId(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(vm, okResult.Value);
    }
}
