using AutoMapper;

namespace OpMetrics.Core.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Vazio: os mapeamentos agora são manuais, dentro de cada slice (ex.: OeeResposta.De).
    }
}
