using FluentValidation;
using OpMetrics.Core.DTOs.Requests;

namespace OpMetrics.Core.Validators;


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

        RuleFor(x => x)
            .Must(x => x.TotalDefeitos + x.TotalRejeitos <= x.TotalInspecionado)
            .WithName("TotalDefeitos")
            .WithMessage("Defeitos + rejeitos não pode ser maior que o total inspecionado");
    }
}

public class CreateQualidadeValidator : AbstractValidator<CreateQualidadeRequest>
{
    public CreateQualidadeValidator()
    {
        Include(new QualidadeRequestRegras());
    }
}

public class UpdateQualidadeValidator : AbstractValidator<UpdateQualidadeRequest>
{
    public UpdateQualidadeValidator()
    {
        Include(new QualidadeRequestRegras());
    }
}
