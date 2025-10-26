using System.Net;
using System.Net.Http.Headers;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

public class TransporteApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly string _schemaPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "IntegrationTests", "Schemas", "transporte-schema.json"));


    public TransporteApiTests(WebApplicationFactory<Program> factory)
    {
        // Substitua pela URL da sua API
        _client = factory.CreateClient();
        _client.BaseAddress = new Uri("https://localhost:8081/api/v1.1/transporte/");
    }

    [Fact]
    public async Task PostTransporte_Valido_DeveRetornar201EConformeSchema()
    {
        // Arrange
        var token = await ObterTokenTeste();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var data = DateOnly.FromDateTime(DateTime.Now);

        var dado = new
        {
            valorPesoKg = 120,
            valorDistanciaKm = 50,
            metodoTransporte = "Truck",
            dataEstimacao = data,
            carbonoKg = 10
        };
        var jsonContent = new StringContent(JsonConvert.SerializeObject(dado), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("", jsonContent);

        // Assert - Status code
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        // Assert - Corpo JSON
        var responseBody = await response.Content.ReadAsStringAsync();
        var obj = JObject.Parse(responseBody);
        ((decimal)obj["valorPesoKg"]).Should().Be(120);
        ((decimal)obj["valorDistanciaKm"]).Should().Be(50);
        ((decimal)obj["carbonoKg"]).Should().Be(10);
        ((string)obj["metodoTransporte"]).Should().Be("Truck");
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
            valorPesoKg = 120,
            valorDistanciaKm = -50,
            metodoTransporte = "Truck",
            dataEstimacao = data,
            carbonoKg = 10
        };

        var jsonContent = new StringContent(JsonConvert.SerializeObject(dado), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("", jsonContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var responseBody = await response.Content.ReadAsStringAsync();
        responseBody.Should().Contain("Distância inválida");
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
