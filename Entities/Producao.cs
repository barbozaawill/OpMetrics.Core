namespace OpMetrics.Core.Entities;

public class Producao : BaseEntity
{
    public string Linha { get; set; } = string.Empty;
    public DateTime Data {  get; set; } = DateTime.UtcNow;
    public string Turno { get; set; } = string.Empty;
    public double MetaPecas { get; set; }
    public double PecasProduzidas { get; set; }
    public double PercentualAtingido => MetaPecas > 0 ? (PecasProduzidas / MetaPecas) * 100 : 0;

}
