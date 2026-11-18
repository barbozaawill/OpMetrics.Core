using FluentValidation;

namespace OpMetrics.Core.Features.Qualidades;

/// <summary>
/// Os dados de entrada que registrar e atualizar uma inspeção de qualidade têm em comum.
/// Só leitura: as regras de validação apenas leem os valores.
/// </summary>
public interface IQualidadeRequest
{
    string Linha { get; }
    DateTime Data { get; }
    string Turno { get; }
    int TotalInspecionado { get; }
    int TotalDefeitos { get; }
    int TotalRejeitos { get; }
}

/// <summary>
/// Regras de validação de qualidade, escritas uma vez e reaproveitadas
/// pelos slices RegistrarQualidade e AtualizarQualidade (via Include).
/// </summary>
public class QualidadeRequestRegras : AbstractValidator<IQualidadeRequest>
{
    public QualidadeRequestRegras()
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

        RuleFor(x => x.TotalInspecionado)
            .GreaterThan(0).WithMessage("O valor do total de peças inspecionadas não pode ser zero.");

        RuleFor(x => x.TotalDefeitos)
            .GreaterThanOrEqualTo(0).WithMessage("Total de defeitos não pode ser negativo");

        RuleFor(x => x.TotalRejeitos)
            .GreaterThanOrEqualTo(0).WithMessage("Total de rejeitos não pode ser negativo");

        // Regra que olha dois campos juntos: defeitos e rejeitos somados
        // não podem passar do total inspecionado.
        RuleFor(x => x)
            .Must(x => x.TotalDefeitos + x.TotalRejeitos <= x.TotalInspecionado)
            .WithName("TotalDefeitos")
            .WithMessage("Defeitos + rejeitos não pode ser maior que o total inspecionado");
    }
}
