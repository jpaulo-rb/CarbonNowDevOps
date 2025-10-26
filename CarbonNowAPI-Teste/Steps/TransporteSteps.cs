using CarbonNowAPI.Model;
using FluentAssertions;
using TechTalk.SpecFlow;

[Binding]
public class TransporteSteps
{
    private readonly FakeDatabase _db = new();
    private ApiResponse _resposta;
    private Transporte _transporte;

    [Given(@"que recebi um transporte com distância ""(.*)"" km")]
    public void DadoQueRecebiUmTransporte(decimal distancia)
    {
        _transporte = new Transporte
        {
            ValorDistanciaKm = distancia,
            ValorPesoKg = 100,
            DataEstimacao = DateTime.Now,
            MetodoTransporte = MetodoTransporte.Truck,
            CarbonoKg = 100
        };
    }

    [Given(@"que recebi um transporte com peso ""(.*)"" kg")]
    public void DadoQueRecebiUmTransporteComPeso(decimal peso)
    {
        _transporte = new Transporte
        {
            ValorDistanciaKm = 100,
            ValorPesoKg = peso,
            DataEstimacao = DateTime.Now,
            MetodoTransporte = MetodoTransporte.Truck,
            CarbonoKg = 100
        };
    }

    [When(@"o sistema processar o registro de transporte")]
    public void QuandoSistemaProcessarRegistroDeTransporte()
    {
        if (_transporte.ValorDistanciaKm <= 0)
            _resposta = new ApiResponse(400, "Distância inválida");
        else if (_transporte.ValorPesoKg <= 0)
            _resposta = new ApiResponse(400, "Peso inválido");
        else
        {
            _db.Salvar(_transporte);
            _resposta = new ApiResponse(201);
        }
    }

    [Then(@"o dado de transporte deve ser salvo no banco de dados e retornar status ""(.*)""")]
    public void EntaoSalvoNoBanco(int status)
    {
        _resposta.StatusCode.Should().Be(status);
        _db.Registros.Should().Contain(_transporte);
    }

    [Then(@"o sistema deve rejeitar o dado de transporte e retornar status ""(.*)"" com mensagem ""(.*)""")]
    public void EntaoSistemaDeveRejeitar(int status, string mensagem)
    {
        _resposta.StatusCode.Should().Be(status);
        _resposta.Mensagem.Should().Be(mensagem);
    }
}