using FluentValidation;

namespace OpMetrics.Core.Features.Oees;

/// <summary>
/// Os dados de entrada que registrar e atualizar um OEE têm em comum.
/// Só leitura: as regras de validação apenas leem os valores.
/// </summary>
public interface IOeeRequest
{
    string Linha { get; }
    DateTime Data { get; }
    string Turno { get; }
    double TempoPlaneadoMinutos { get; }
    double TempoRodandoMinutos { get; }
    double CapacidadeIdealPecas { get; }
    double PecasProduzidas { get; }
    double PecasBoas { get; }
}

/// <summary>
/// Regras de validação de um OEE, escritas uma vez e reaproveitadas
/// pelos slices RegistrarOee e AtualizarOee (via Include).
/// </summary>
public class OeeRequestRegras : AbstractValidator<IOeeRequest>
{
    public OeeRequestRegras()
    {
        RuleFor(x => x.Linha)
            .NotEmpty().WithMessage("Linha é obrigatória")
            .MaximumLength(100).WithMessage("Linha deve ter no máximo 100 caracteres");

        RuleFor(x => x.Data)
            .NotEmpty().WithMessage("Data é obrigatória")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Data não pode ser futura");

        RuleFor(x => x.Turno)
            .NotEmpty().WithMessage("Turno é obrigatório")
            .Must(t => t == "Manha" || t == "Tarde" || t == "Noite")
            .WithMessage("Turno deve ser: Manha, Tarde ou Noite.");

        RuleFor(x => x.TempoPlaneadoMinutos)
            .GreaterThan(0).WithMessage("Tempo planeado deve ser maior que zero");

        RuleFor(x => x.TempoRodandoMinutos)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Tempo rodando não pode ser menor que zero")
            .LessThanOrEqualTo(x => x.TempoPlaneadoMinutos).WithMessage("Tempo rodando não pode ser maior que o tempo planeado");

        RuleFor(x => x.CapacidadeIdealPecas)
            .GreaterThan(0).WithMessage("Capacidade ideal deve ser maior que zero");

        RuleFor(x => x.PecasProduzidas)
            .GreaterThanOrEqualTo(0).WithMessage("Peças produzidas não pode ser menor que zero");

        RuleFor(x => x.PecasBoas)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Peças boas não pode ser menor que zero")
            .LessThanOrEqualTo(x => x.PecasProduzidas).WithMessage("Peças boas não pode ser maior que peças produzidas");
    }
}
