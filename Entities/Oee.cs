namespace OpMetrics.Core.Entities;

public class Oee : BaseEntity
{
    public string Linha { get; set; } = string.Empty;
    public DateTime Data { get; set; } = DateTime.UtcNow;
    public string Turno { get; set; } = string.Empty;
    public double TempoPlaneadoMinutos { get; set; }
    public double TempoRodandoMinutos { get; set; }
    public double Disponibilidade => TempoPlaneadoMinutos > 0 ? (TempoRodandoMinutos / TempoPlaneadoMinutos) * 100 : 0;
    public double CapacidadeIdealPecas { get; set; }
    public double PecasProduzidas { get; set; }
    public double Performance => CapacidadeIdealPecas > 0 ? (PecasProduzidas / CapacidadeIdealPecas) * 100 : 0 ;
    public double PecasBoas { get; set; }
    public double IndiceQualidade => IndiceQualidade > 0 ? (PecasBoas / PecasProduzidas) * 100 : 0;
    public double OeePercentual => (Disponibilidade / 100) * (Performance / 100) * (IndiceQualidade / 100) * 100;

}
