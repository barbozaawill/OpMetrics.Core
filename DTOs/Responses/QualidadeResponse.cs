namespace OpMetrics.Core.DTOs.Responses;

public class QualidadeResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Linha { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public string Turno { get; set; } = string.Empty;
    public int TotalInspecionado { get; set; } = 0;
    public int TotalDefeitos { get; set; } = 0;
    public int TotalRejeitos { get; set; } = 0;
    public double IndiceQualidade {get; set; }
}
