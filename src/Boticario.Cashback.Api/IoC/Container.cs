using Boticario.Cashback.Api.Middlewares;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Boticario.Cashback.Api.IoC
{
    public static class Container
    {
        public static void AddApplicationServicesWeb(this IServiceCollection services)
        {
            services.AddUsuarioLogado();
            services.AddMediatR();
        }

        private static void AddUsuarioLogado(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
        }

        private static void AddMediatR(this IServiceCollection services)
        {
            var assembly = AppDomain.CurrentDomain.Load("Boticario.Cashback.Dominio");
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestsValidationMiddleware<,>));
            services.AddMediatR(assembly);
        }
    }
}
