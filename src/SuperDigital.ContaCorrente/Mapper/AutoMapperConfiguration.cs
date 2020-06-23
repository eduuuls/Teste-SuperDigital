using AutoMapper;

namespace SuperDigital.ContaCorrente.API.Mapper
{
    public static class AutoMapperConfiguration
    {

        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(ps =>
            {
                ps.AddProfile(new ModelToViewModel());
                ps.AddProfile(new ViewModelToModel());
            });
        }
    }
}
