using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OpMetrics.Core.Data;
using OpMetrics.Core.Features.Oees;
using OpMetrics.Core.Features.Producoes;
using OpMetrics.Core.Features.Qualidades;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registra todos os validators do projeto (inclusive os que ficam dentro dos slices).
// Quem executa a validação é o ValidationFilter (Common/ValidationFilter.cs).
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Vertical slices: cada feature registra os próprios handlers.
builder.Services.AddOee();
builder.Services.AddProducao();
builder.Services.AddQualidade();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Cada slice tem sua própria classe "Request" (RegistrarOee.Request, AtualizarOee.Request...).
    // Sem isso, o Swagger tentaria chamar todas de "Request" e daria conflito.
    // Com isso, elas viram "RegistrarOeeRequest", "AtualizarOeeRequest" etc.
    options.CustomSchemaIds(tipo =>
        tipo.DeclaringType is null ? tipo.Name : tipo.DeclaringType.Name + tipo.Name);
});

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Vertical slices: cada feature registra as próprias rotas.
app.MapOee();
app.MapProducao();
app.MapQualidade();

app.Run();

// Permite que os testes referenciem o assembly da API (ex.: WebApplicationFactory<Program>).
public partial class Program { }
