using Boticario.Cashback.Api.Extensions;
using Boticario.Cashback.Api.IoC;
using Boticario.Cashback.Api.Middlewares;
using Boticario.Cashback.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using System;

namespace Boticario.Cashback.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(c =>
                {
                    c.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                });


            services.AddHealthChecks()
                .AddMongoDb(Configuration["Db:ConnectionString"],
                    mongoDatabaseName: Configuration["Db:Nome"],
                    name: "MongoDb",
                    failureStatus: HealthStatus.Degraded,
                    tags: new string[] { "db", "MongoDb" });

            services.AddCors();
            services.AddApplicationServices(Configuration);
            services.AddApplicationServicesWeb();
            services.AddAuthorizedMvc();

            services.AddHttpClient("boticario", client =>
            {
                client.BaseAddress = new Uri("https://mdaqk8ek5j.execute-api.us-east-1.amazonaws.com/");
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseError();
            app.UseCors(builder => builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod());
            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHealthChecks("/health");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
