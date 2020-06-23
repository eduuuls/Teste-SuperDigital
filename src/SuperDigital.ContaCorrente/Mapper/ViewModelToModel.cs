using AutoMapper;
using SuperDigital.ContaCorrente.API.ViewModels;
using SuperDigital.ContaCorrente.Domain.Entidades;

namespace SuperDigital.ContaCorrente.API.Mapper
{

    public class ViewModelToModel : Profile
    {
        public ViewModelToModel()
        {
            CreateMap<ContaCorrenteViewModel, Conta>();
        }
    }
}
