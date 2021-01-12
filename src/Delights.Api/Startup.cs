using Delights.Modules;
using Delights.Modules.Hello.Server;
using Delights.Modules.ModuleManager.Server;
using Modulight.Modules.Server.GraphQL;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Modulight.Modules;

namespace Delights.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Delights.Api", Version = "v1" });
            });

            services.AddCors();

            var builder = ModuleHostBuilder.CreateDefaultBuilder()
                .AddGraphQLServerModules()
                .AddHelloModule()
                .AddModuleManagerModule();

            builder.Build(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Delights.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors();

            var graphQLModule = app.ApplicationServices.GetCoreGraphQLServerModule().GetService(app.ApplicationServices);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                graphQLModule.MapEndpoints(endpoints, (_, builder) =>
                {
                    builder.RequireCors(cors =>
                    {
                        cors.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    });
                });
            });
        }
    }
}
