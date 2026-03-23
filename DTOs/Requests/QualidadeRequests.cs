namespace OpMetrics.Core.DTOs.Requests;

public class CreateQualidadeRequest
{
    public string Linha { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public string Turno { get; set; } = string.Empty;
    public int TotalInspecionado { get; set; } = 0;
    public int TotalDefeitos { get; set; } = 0;
    public int TotalRejeitos { get; set; } = 0;
}

public class UpdateQualidadeRequest
{
    public string Linha { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public string Turno { get; set; } = string.Empty;
    public int TotalInspecionado { get; set; } = 0;
    public int TotalDefeitos { get; set; } = 0;
    public int TotalRejeitos { get; set; } = 0;
}
