using AutoMapper;
using OpMetrics.Core.DTOs.Requests;
using OpMetrics.Core.DTOs.Responses;
using OpMetrics.Core.Entities;

namespace OpMetrics.Core.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Qualidade
        CreateMap<CreateQualidadeRequest, Qualidade>();
        CreateMap<UpdateQualidadeRequest, Qualidade>();

        CreateMap<Qualidade, QualidadeResponse>();
    }
}
