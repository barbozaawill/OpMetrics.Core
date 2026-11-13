namespace OpMetrics.Core.DTOs.Requests;

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

public class UpdateOeeRequest : IOeeRequest
{
    public string Linha { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public string Turno { get; set; } = string.Empty;
    public double TempoPlaneadoMinutos { get; set; }
    public double TempoRodandoMinutos { get; set; }
    public double CapacidadeIdealPecas { get; set; }
    public double PecasProduzidas { get; set; }
    public double PecasBoas { get; set; }

}
