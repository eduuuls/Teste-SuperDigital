using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SuperDigital.ContaCorrente.Domain.Interfaces.Repositories;
using SuperDigital.ContaCorrente.Infra.Data.Repositories;
using SuperDigital.ContaCorrente.Domain.Interfaces.Services;
using SuperDigital.ContaCorrente.Domain.Services;
using SuperDigital.ContaCorrente.Infra.Data.Context;
using SuperDigital.ContaCorrente.Infra.Data.UoW;
using SuperDigital.ContaCorrente.Domain.Interfaces.UnitOfWork;
using SuperDigital.ContaCorrente.Domain.Entidades;

namespace SuperDigital.ContaCorrente.Infra.CrossCutting.IoC
{
    public static class BootsTrapper
    {
        public static void RegistrarDI(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ContaCorrenteContext<Conta>>();
            services.AddScoped<IUnitOfWork, UnitOfWorkContaCorrente>();
            RegistraRepositories(services);
            RegistraServices(services);
        }

        static void RegistraRepositories(IServiceCollection services)
        {
            services.AddScoped<IContaCorrenteRepository, ContaCorrenteRepository>();
        }

        static void RegistraServices(IServiceCollection services)
        {
            services.AddScoped<IMovimentacaoService, MovimentacaoService>();
            services.AddScoped<IContaCorrenteService, ContaCorrenteService>();
        }
    }
}
