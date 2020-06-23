using AutoMapper;
using SuperDigital.ContaCorrente.API.ViewModels;
using SuperDigital.ContaCorrente.Domain.Entidades;

namespace SuperDigital.ContaCorrente.API.Mapper
{

    public class ModelToViewModel : Profile
    {
        public ModelToViewModel()
        {
            CreateMap<Conta, ContaCorrenteViewModel>();
        }
    }
}
