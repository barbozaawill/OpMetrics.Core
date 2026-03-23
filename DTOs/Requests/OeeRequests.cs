namespace OpMetrics.Core.DTOs.Requests;

public class CreateOeeRequest
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

public class UpdateOeeRequest
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
