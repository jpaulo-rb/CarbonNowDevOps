using CarbonNowAPI.Model;
using FluentAssertions;
using TechTalk.SpecFlow;

[Binding]
public class EletricidadeSteps
{
    private readonly FakeDatabase _db = new();
    private ApiResponse _resposta;
    private Eletricidade _eletricidade;

    [Given(@"que recebi um dado de eletricidade com valor de eletricidade ""(.*)""")]
    public void DadoQueRecebiDadoEletricidade(decimal valor)
    {
        _eletricidade = new Eletricidade
        {
            UnidadeEletricidade = UnidadeEletricidade.KWH,
            DataEstimacao = DateTime.Now,
            ValorEletricidade = valor,
            CarbonoKg = 50
        };
    }

    [Given(@"que recebi um dado de eletricidade com valor de carbono ""(.*)"" kg")]
    public void DadoQueRecebiDadoCarbono(decimal carbonoKg)
    {
        _eletricidade = new Eletricidade
        {
            UnidadeEletricidade = UnidadeEletricidade.KWH,
            DataEstimacao = DateTime.Now,
            ValorEletricidade = 100,
            CarbonoKg = carbonoKg
        };
    }

    [When(@"o sistema processar o registro de eletricidade")]
    public void QuandoSistemaProcessarRegistroDeEletricidade()
    {
        if (_eletricidade.ValorEletricidade <= 0)
            _resposta = new ApiResponse(400, "Valor de eletricidade inválido");
        else if (_eletricidade.CarbonoKg <= 0)
            _resposta = new ApiResponse(400, "Valor de carbono inválido");
        else
        {
            _db.Salvar(_eletricidade);
            _resposta = new ApiResponse(201);
        }
    }

    [Then(@"o dado de eletricidade deve ser salvo no banco de dados e retornar status ""(.*)""")]
    public void EntaoSalvoNoBanco(int status)
    {
        _resposta.StatusCode.Should().Be(status);
        _db.Registros.Should().Contain(_eletricidade);
    }

    [Then(@"o sistema deve rejeitar o dado de eletricidade e retornar status ""(.*)"" com mensagem ""(.*)""")]
    public void EntaoSistemaDeveRejeitar(int status, string mensagem)
    {
        _resposta.StatusCode.Should().Be(status);
        _resposta.Mensagem.Should().Be(mensagem);
    }
}