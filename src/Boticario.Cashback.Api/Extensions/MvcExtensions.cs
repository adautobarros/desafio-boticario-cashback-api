using Boticario.Cashback.Api.Policies;
using Boticario.Cashback.Dominio.Entidades;
using Boticario.Cashback.Dominio.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Boticario.Cashback.Api.Extensions
{
    public static class MvcExtensions
    {
        public static void AddAuthorizedMvc(this IServiceCollection services)
        {
            AddJwtAuthorization(services);
            AddMvc(services);
        }

        private static void AddJwtAuthorization(IServiceCollection services)
        {
            var jwtSettings = services.BuildServiceProvider().GetRequiredService<JwtSettings>();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(jwtBearerOptions =>
                {
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateActor = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = jwtSettings.SigningCredentials.Key
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteUserPolicy", policy =>
                    policy.Requirements.Add(new DeleteUserRequirement("CanDeleteUser")));
            });

            services.AddSingleton<IAuthorizationHandler, DeleteUserRequirementHandler>();
        }

        private static void AddMvc(IServiceCollection services)
        {
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                config.Filters.Add(new AuthorizeFilter(policy));
            });
        }
    }
}
