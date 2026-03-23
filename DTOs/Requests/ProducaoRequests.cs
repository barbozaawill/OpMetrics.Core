namespace OpMetrics.Core.DTOs.Requests;

public class CreateProducaoRequest
{
    public string Linha { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public string Turno { get; set; } = string.Empty;
    public double MetaPecas { get; set; }
    public double PecasProduzidas { get; set; }
}

public class UpdateProducaoRequest
{
    public string Linha { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public string Turno { get; set; } = string.Empty;
    public double MetaPecas { get; set; }
    public double PecasProduzidas { get; set; }
}

