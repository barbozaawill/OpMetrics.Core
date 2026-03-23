using AutoMapper;
using OpMetrics.Core.DTOs.Requests;
using OpMetrics.Core.DTOs.Responses;
using OpMetrics.Core.Entities;

namespace OpMetrics.Core.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {

        //Producao
        CreateMap<CreateProducaoRequest, Producao>();
        CreateMap<UpdateProducaoRequest, Producao>();

        CreateMap<Producao, ProducaoResponse>();

        // Qualidade
        CreateMap<CreateQualidadeRequest, Qualidade>();
        CreateMap<UpdateQualidadeRequest, Qualidade>();

        CreateMap<Qualidade, QualidadeResponse>();

        //Oee
        CreateMap<CreateOeeRequest, Oee>();
        CreateMap<UpdateOeeRequest, Oee>();

        CreateMap<Oee, OeeResponse>();
    }
}
