using System.Net;
using System.Net.Http.Headers;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

public class EletricidadeApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly string _schemaPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "IntegrationTests", "Schemas", "eletricidade-schema.json"));


    public EletricidadeApiTests(WebApplicationFactory<Program> factory)
    {
        // Substitua pela URL da sua API
        _client = factory.CreateClient();
        _client.BaseAddress = new Uri("https://localhost:8081/api/v1.1/eletricidade/");
    }

    [Fact]
    public async Task PostEletricidade_Valido_DeveRetornar201EConformeSchema()
    {
        // Arrange

        var token = await ObterTokenTeste();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var data = DateOnly.FromDateTime(DateTime.Now);

        var dado = new
        {
            valorEletricidade = 120,
            carbonoKg = 50,
            dataEstimacao = data,
            unidadeEletricidade = "KWH"
        };
        var jsonContent = new StringContent(JsonConvert.SerializeObject(dado), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("", jsonContent);

        // Assert - Status code
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        // Assert - Corpo JSON
        var responseBody = await response.Content.ReadAsStringAsync();
        var obj = JObject.Parse(responseBody);
        ((decimal)obj["valorEletricidade"]).Should().Be(120);
        ((decimal)obj["carbonoKg"]).Should().Be(50);
        ((string)obj["unidadeEletricidade"]).Should().Be("KWH");
        DateOnly.Parse(obj["dataEstimacao"].ToString()).Should().Be(data);

        // Assert - JSON Schema
        var schema = JSchema.Parse(File.ReadAllText(_schemaPath));
        obj.IsValid(schema).Should().BeTrue("O JSON de resposta deve estar conforme o contrato do schema");
    }

    [Fact]
    public async Task PostEletricidade_ValorInvalido_DeveRetornar400()
    {
        // Arrange
        var token = await ObterTokenTeste();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var data = DateOnly.FromDateTime(DateTime.Now);

        var dado = new
        {
            valorEletricidade = -50,
            carbonoKg = 50,
            dataEstimacao = data,
            unidadeEletricidade = "KWH"
        };
        var jsonContent = new StringContent(JsonConvert.SerializeObject(dado), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("", jsonContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var responseBody = await response.Content.ReadAsStringAsync();
        responseBody.Should().Contain("Valor de eletricidade inválido");
    }

    private async Task<string> ObterTokenTeste()
    {
        var login = new { email = "user@example.com", senha = "string" };
        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(login), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/v1.1/usuario/login", content);

        var token = await response.Content.ReadAsStringAsync();
        return token;
    }
}
