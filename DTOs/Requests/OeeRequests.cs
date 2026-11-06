namespace OpMetrics.Core.DTOs.Requests;

public interface IOeeRequest
{
    string Linha { get; set; }
    DateTime Data { get; set; }
    string Turno { get; set; }
    double TempoPlaneadoMinutos { get; set; }
    double TempoRodandoMinutos { get; set; }
    double CapacidadeIdealPecas { get; set; }
    double PecasProduzidas { get; set; }
    double PecasBoas { get; set; }
}

public class CreateOeeRequest : IOeeRequest
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
