namespace Api
{
    using Api.Filters;
    using Application;
    using Common;
    using Communication;
    using Domain;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.IdentityModel.Logging;
    using Persistance;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWebApi(Configuration);
            services.AddPersistance(Configuration);
            services.AddCommunication(Configuration);
            services.AddCommon(Configuration);
            services.AddDomain(Configuration);
            services.AddApplication(Configuration);

            services.AddControllers(options =>
                options.Filters.Add(new ApiExceptionFilter()));

            //Console log visibility
            IdentityModelEventSource.ShowPII = true;
            services.AddCors();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });

            app.UseWebApi(Configuration, provider);
        }
    }
}