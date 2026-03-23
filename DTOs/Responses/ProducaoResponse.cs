namespace OpMetrics.Core.DTOs.Responses;

public class ProducaoResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Linha { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public string Turno { get; set; } = string.Empty;
    public double MetaPecas { get; set; }
    public double PecasProduzidas { get; set; }
    public double PercentualAtingido { get; set; }
}
