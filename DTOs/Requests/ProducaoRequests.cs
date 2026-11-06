namespace OpMetrics.Core.DTOs.Requests;

public interface IProducaoRequest
{
    string Linha { get; set; }
    DateTime Data { get; set; }
    string Turno { get; set; }
    double MetaPecas { get; set; }
    double PecasProduzidas { get; set; }
}

public class CreateProducaoRequest : IProducaoRequest
{
    public string Linha { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public string Turno { get; set; } = string.Empty;
    public double MetaPecas { get; set; }
    public double PecasProduzidas { get; set; }
}

public class UpdateProducaoRequest : IProducaoRequest
{
    public string Linha { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public string Turno { get; set; } = string.Empty;
    public double MetaPecas { get; set; }
    public double PecasProduzidas { get; set; }
}

