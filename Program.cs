using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OpMetrics.Core.Data;
using OpMetrics.Core.Mappings;
using OpMetrics.Core.Repositories;
using OpMetrics.Core.Repositories.Interfaces;
using OpMetrics.Core.Services;
using OpMetrics.Core.Validators;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddValidatorsFromAssemblyContaining<CreateProducaoValidator>();

//repositorios
builder.Services.AddScoped<IProducaoRepository, ProducaoRepository>();
builder.Services.AddScoped<IQualidadeRepository, QualidadeRepository>();
builder.Services.AddScoped<IOeeRepository, OeeRepository>();

//serviços
builder.Services.AddScoped<IProducaoService, ProducaoService>();
builder.Services.AddScoped<IQualidadeService, QualidadeService>();
builder.Services.AddScoped<IOeeService, OeeService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();