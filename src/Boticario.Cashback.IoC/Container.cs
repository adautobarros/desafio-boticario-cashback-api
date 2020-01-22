using Boticario.Cashback.Dominio.Entidades;
using Boticario.Cashback.Dominio.Model;
using Boticario.Cashback.Dominio.Repositorios;
using Boticario.Cashback.Dominio.Servicos;
using Boticario.Cashback.Infra.Context;
using Boticario.Cashback.Infra.Repositorios;
using Boticario.Cashback.Infra.Servicos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Boticario.Cashback.IoC
{
    public static class Container
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)

        {
            services.AddRepositories();
            services.AddMongoConnection(configuration);
            services.AddServices(configuration);
            return services;
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IRevendedorRepositorio, RevendedorRepositorio>();
            services.AddScoped<ICompraRepositorio, CompraRepositorio>();
        }

        private static void AddMongoConnection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(typeof(MongoDbContext), serviceProvider =>
              new MongoDbContext(configuration["Db:ConnectionString"], configuration["Db:Nome"])
          );
        }

        private static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<JwtSettings>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IBoticarioApiService, BoticarioApiService>();
        }
    }
}
