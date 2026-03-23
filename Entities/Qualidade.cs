namespace OpMetrics.Core.Entities;

public class Qualidade : BaseEntity
{
    public string Linha { get; set; } = string.Empty;
    public DateTime Data {  get; set; } = DateTime.UtcNow;
    public string Turno { get; set; } = string.Empty;
    public int TotalInspecionado { get; set; } = 0;
    public int TotalDefeitos { get; set; } = 0;
    public int TotalRejeitos { get; set; } = 0;
    public double IndiceQualidade => TotalInspecionado > 0 ? ((TotalInspecionado - TotalDefeitos - TotalRejeitos) / (double)TotalInspecionado) * 100 : 0;
}
