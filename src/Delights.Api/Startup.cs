using Delights.Modules.Bookkeeping.Server;
using Delights.Modules.Notes.Server;
using Delights.Modules.Persons.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Modulight.Modules.Hosting;
using Modulight.Modules.Server.AspNet;
using Modulight.Modules.Server.GraphQL;
using StardustDL.AspNet.IdentityServer;
using StardustDL.AspNet.ItemMetadataServer;
using StardustDL.AspNet.ObjectStorage;

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

            var builder = ModuleHostBuilder.CreateDefaultBuilder().UseGraphQLServerModules();

            /*builder.AddIdentityServerModule((o, sp) =>
            {
                o.ConfigureDbContext = options =>
                    options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection"));
                o.ConfigureIdentity = options => options.SignIn.RequireConfirmedAccount = false;
                o.ConfigureIdentityServer = options => Configuration.GetSection("IdentityServer:Options").Bind(options);
                o.ConfigureApiAuthorization = options => Configuration.GetSection("IdentityServer").Bind(options);
            }, (o, sp) =>
            {
                o.JwtAudiences = new string[] { "Delights.ApiAPI" };

                o.InitialUser = new StardustDL.AspNet.IdentityServer.Models.ApplicationUser
                {
                    UserName = "admin@delights",
                    Email = "admin@delights",
                    EmailConfirmed = true,
                    LockoutEnabled = false
                };

                o.InitialUserPassword = "123P$d";
            }).AddIdentityServerGraphqlModule();

            builder.AddObjectStorageModule((o, sp) =>
                {
                    o.Endpoint = "localhost:9000";
                    o.AccessKey = "user";
                    o.SecretKey = "password";
                })
                .AddObjectStorageApiModule();*/

            builder.AddItemMetadataServerModule((o, sp) =>
            {
                o.ConfigureDbContext = options =>
                    options.UseSqlServer(Configuration.GetConnectionString("ItemMetadataConnection"));
            })
                .AddItemMetadataServerGraphqlModule();

            builder
                 .AddPersonsServerModule((o, sp) =>
                 {
                     o.ConfigureDbContext = options =>
                         options.UseSqlServer(Configuration.GetConnectionString("PersonsConnection"));
                 })
                 .AddNotesServerModule((o, sp) =>
                 {
                     o.ConfigureDbContext = options =>
                         options.UseSqlServer(Configuration.GetConnectionString("NotesConnection"));
                 })
                 .AddBookkeepingServerModule((o, sp) =>
                  {
                      o.ConfigureDbContext = options =>
                          options.UseSqlServer(Configuration.GetConnectionString("BookkeepingConnection"));
                  });

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

            app.UseCors();

            // app.UseAspNetServerModuleMiddlewares();

            app.UseEndpoints(endpoints =>
            {
                // endpoints.MapAspNetServerModuleEndpoints();
                endpoints.MapGraphQLServerModuleEndpoints(postMapEndpoint: (module, builder) =>
                {
                    builder.RequireCors(cors =>
                    {
                        cors.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    });
                });
                endpoints.MapControllers();
            });
        }
    }
}
