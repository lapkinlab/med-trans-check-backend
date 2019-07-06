using System;
using System.Threading.Tasks;
using AspNetCore.Identity.Mongo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.DependencyInjection;
using Models.Checkpoints.Repositories;
using Models.Drivers.Repositories;
using Models.MechanicNotes.Repositories;
using Models.MedicNotes.Repositories;
using Models.Vehicles.Repositories;
using Models.Tags.Repositories;
using Models.Roles;
using Models.Routes.Repositories;
using Models.Users;
using Swashbuckle.AspNetCore.Swagger;

namespace API
{
    public class Startup
    {
        private const string DocsRoute = "secret-materials/docs";
        private const string DocName = "MTC-API";
        private const string DocTitle = "MedTransCheck API";
        private const string CorsPolicy = "AllowAnyPolicy";
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ITagRepository, MongoTagRepository>();
            services.AddSingleton<IVehicleRepository, MongoVehicleRepository>();
            services.AddSingleton<IDriverRepository, MongoDriverRepository>();
            services.AddSingleton<ICheckpointRepository, MongoCheckpointRepository>();
            services.AddSingleton<IRouteRepository, MongoRouteRepository>();
            services.AddSingleton<IMedicNoteRepository, MongoMedicNoteRepository>();
            services.AddSingleton<IMechanicNoteRepository, MongoMechanicNoteRepository>();

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.SlidingExpiration = true;
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.Headers["Location"] = context.RedirectUri;
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });
            services.AddIdentityMongoDbProvider<User, Role>(mongo =>
                mongo.ConnectionString = "mongodb://localhost:27017/MedTransCheckDb");
            
            // Any CORS permission
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory(CorsPolicy));
            });
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicy, builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            
            services.AddMvc();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(DocName, new Info
                {
                    Version = "v1",
                    Title = DocTitle,
                    Description = "ASP.NET Core Web API"
                });
                options.IncludeXmlComments($"{AppDomain.CurrentDomain.BaseDirectory}/API.xml");
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseStatusCodePagesWithReExecute("/index.html");
            app.UseCors(CorsPolicy);
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc();

            app.UseSwagger(options => options.RouteTemplate = $"{DocsRoute}/{{documentName}}/swagger.json");
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = DocsRoute;
                options.SwaggerEndpoint($"/{DocsRoute}/{DocName}/swagger.json", DocTitle);
            });
        }
    }
}