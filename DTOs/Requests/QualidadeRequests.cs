namespace OpMetrics.Core.DTOs.Requests;

// Create e Update tem os mesmos campos, entao compartilham esta interface.
// Assim as regras de validacao sao escritas uma vez so (ver Validators/QualidadeValidator.cs).
public interface IQualidadeRequest
{
    string Linha { get; set; }
    DateTime Data { get; set; }
    string Turno { get; set; }
    int TotalInspecionado { get; set; }
    int TotalDefeitos { get; set; }
    int TotalRejeitos { get; set; }
}

public class CreateQualidadeRequest : IQualidadeRequest
{
    public string Linha { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public string Turno { get; set; } = string.Empty;
    public int TotalInspecionado { get; set; } = 0;
    public int TotalDefeitos { get; set; } = 0;
    public int TotalRejeitos { get; set; } = 0;
}

public class UpdateQualidadeRequest : IQualidadeRequest
{
    public string Linha { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public string Turno { get; set; } = string.Empty;
    public int TotalInspecionado { get; set; } = 0;
    public int TotalDefeitos { get; set; } = 0;
    public int TotalRejeitos { get; set; } = 0;
}
