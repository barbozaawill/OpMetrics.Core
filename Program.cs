using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using OpMetrics.Core.Data;
using OpMetrics.Core.Features.Oees;
using OpMetrics.Core.Features.Producoes;
using OpMetrics.Core.Features.Qualidades;
using OpMetrics.Core.Mappings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
// Não há mais controllers: os slices validam pelo ValidationFilter (Common/ValidationFilter.cs).
// A validação automática de controllers e o AutoMapper saem no próximo commit (limpeza final).
builder.Services.AddFluentValidationAutoValidation();

// Vertical slices
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
app.UseAuthorization();
app.MapControllers();
app.MapOee();
app.MapProducao();
app.MapQualidade();

app.Run();

// Permite que os testes referenciem o assembly da API (ex.: WebApplicationFactory<Program>).
public partial class Program { }
