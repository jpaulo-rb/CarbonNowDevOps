using System.Text;
using System.Text.Json.Serialization;
using Asp.Versioning;
using CarbonNowAPI.Data;
using CarbonNowAPI.Repository;
using CarbonNowAPI.Service;
using CarbonNowAPI.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Configuração para a rota [controller] ficar minuscula
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddControllers()
    // Configuração para enums serem passados como string
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();


// Configuração do Swagger
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(s =>
{

    s.OperationFilter<SwaggerDefaultValues>();

    // Configuração para o Swagger não especificar Enums
    s.UseInlineDefinitionsForEnums();


    // Configurações para informar o JWT no Swagger
    s.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Insira o token JWT com prefixo 'Bearer '"
    });

    s.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


// Configuração para versionamento
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1.1);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-API-Version"));
})
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });


#region DI Configs
builder.Services.AddDbContext<DatabaseConnection>();
builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddScoped<TokenJWT>();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddScoped<IEletricidadeRepository, EletricidadeRepository>();
builder.Services.AddScoped<IEletricidadeService, EletricidadeService>();

builder.Services.AddScoped<ITransporteRepository, TransporteRepository>();
builder.Services.AddScoped<ITransporteService, TransporteService>();
#endregion

// Configuração necessária para utilizar JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// Configuração regras JWT 
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", regra => regra.RequireRole("admin"));
    options.AddPolicy("Normal", regra => regra.RequireRole("normal", "admin"));
});

var app = builder.Build();

// Migration automática
using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<DatabaseConnection>();
db.Database.Migrate();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    // Configuração do Swagger para versionamento
    app.UseSwaggerUI(options =>
    {
        foreach (var description in app.DescribeApiVersions().OrderByDescending(d => d.ApiVersion))
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName);
        }
    });
}

// Configuração para tratamento de Exceptions
app.UseMiddleware<ExceptionMiddleware>();

// Configuração para o JWT
app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
public partial class Program { }

// Testes