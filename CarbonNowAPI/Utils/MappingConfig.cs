using AutoMapper;
using CarbonNowAPI.Model;
using CarbonNowAPI.ViewModel.Eletricidade;
using CarbonNowAPI.ViewModel.Transporte;
using CarbonNowAPI.ViewModel.Usuario;

namespace CarbonNowAPI.Utils
{
    public class MappingConfig : Profile
    {

        public MappingConfig()
        {


            // Usuario
            CreateMap<Usuario, UsuarioExibicaoViewModel>();
            CreateMap<UsuarioCadastroViewModel, Usuario>();
            CreateMap<UsuarioAlteracaoViewModel, Usuario>();
            CreateMap<UsuarioLoginViewModel, Usuario>();


            // Eletricidade
            CreateMap<Eletricidade, EletricidadeViewModel>()
                .ForMember(dest => dest.DataEstimacao,
                    opt => opt.MapFrom(src => DateOnly.FromDateTime(src.DataEstimacao)));

            CreateMap<EletricidadeViewModel, Eletricidade>()
                .ForMember(dest => dest.DataEstimacao,
                    opt => opt.MapFrom(src => src.DataEstimacao.ToDateTime(TimeOnly.MinValue)));


            CreateMap<EletricidadeCadastroViewModel, Eletricidade>()
                .ForMember(dest => dest.DataEstimacao,
                    opt => opt.MapFrom(src => src.DataEstimacao.ToDateTime(TimeOnly.MinValue)));


            // Transporte
            CreateMap<Transporte, TransporteViewModel>()
                .ForMember(dest => dest.DataEstimacao,
                    opt => opt.MapFrom(src => DateOnly.FromDateTime(src.DataEstimacao)));

            CreateMap<TransporteViewModel, Transporte>()
                .ForMember(dest => dest.DataEstimacao,
                    opt => opt.MapFrom(src => src.DataEstimacao.ToDateTime(TimeOnly.MinValue)));

            CreateMap<TransporteCadastroViewModel, Transporte>()
                .ForMember(dest => dest.DataEstimacao,
                    opt => opt.MapFrom(src => src.DataEstimacao.ToDateTime(TimeOnly.MinValue)));

        }
    }
}
