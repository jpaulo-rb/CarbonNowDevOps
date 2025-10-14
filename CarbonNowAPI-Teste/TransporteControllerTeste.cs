using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CarbonNowAPI.Controller;
using CarbonNowAPI.Model;
using CarbonNowAPI.Service;
using CarbonNowAPI.ViewModel;
using CarbonNowAPI.ViewModel.Transporte;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class TransporteControllerTests {
    [Fact]
    public async Task ListarTodosTransportes_RetornaOkComPaginacao() {
        // Arrange
        var mockService = new Mock<ITransporteService>();
        var mockMapper = new Mock<IMapper>();

        var pagina = 1;
        var tamanho = 20;
        var baseUrl = "http://localhost/api/v1.1/transporte";

        var resultadoService = new Paginacao<Transporte> {
            PaginaAtual = pagina,
            TamanhoPagina = tamanho,
            TotalItens = 100,
            Itens = new List<Transporte>(), // pode colocar itens mock aqui
            ProximaPaginaUrl = $"{baseUrl}?pagina=2&tamanho={tamanho}",
            PaginaAnteriorUrl = null
        };

        mockService
            .Setup(s => s.ListarTransportes(pagina, tamanho, It.IsAny<string>()))
            .ReturnsAsync(resultadoService);

        mockMapper
            .Setup(m => m.Map<List<TransporteViewModel>>(It.IsAny<List<Transporte>>()))
            .Returns(new List<TransporteViewModel>());

        var controller = new TransporteController(mockService.Object, mockMapper.Object);

        // Configurar Request para baseUrl
        controller.ControllerContext = new ControllerContext();
        controller.ControllerContext.HttpContext = new DefaultHttpContext();
        controller.ControllerContext.HttpContext.Request.Scheme = "http";
        controller.ControllerContext.HttpContext.Request.Host = new HostString("localhost");
        controller.ControllerContext.HttpContext.Request.Path = "/api/v1.1/transporte";

        // Act
        var result = await controller.ListarTodosTransportes(pagina, tamanho);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, okResult.StatusCode);

        var paginacao = Assert.IsType<Paginacao<TransporteViewModel>>(okResult.Value);
        Assert.Equal(pagina, paginacao.PaginaAtual);
        Assert.Equal(tamanho, paginacao.TamanhoPagina);
    }
}
