namespace OpMetrics.Core.DTOs.Responses;

public class OeeResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Linha { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public string Turno { get; set; } = string.Empty;
    public double TempoPlaneadoMinutos { get; set; }
    public double TempoRodandoMinutos { get; set; }
    public double Disponibilidade { get; set; }
    public double CapacidadeIdealPecas { get; set; }
    public double PecasProduzidas { get; set; }
    public double Performance {get; set;}
    public double PecasBoas { get; set; }
    public double IndiceQualidade {get; set; }
    public double OeePercentual {get; set;}
}
