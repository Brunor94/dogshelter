using System;
using System.IO;
using DogShelterService.Api.Registers;
using DogShelterService.Common.Services;
using DogShelterService.Common.Services.Interfaces;
using DogShelterService.Domain.Repositories.Interfaces;
using DogShelterService.Infrastructure.Configurations;
using DogShelterService.Infrastructure.Configurations.Interfaces;
using DogShelterService.Infrastructure.Persistence;
using DogShelterService.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace DogShelterService.Api
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var retryPolicyConfiguration = new RetryPolicyConfiguration();
            this.Configuration.GetSection("RetryPolicy").Bind(retryPolicyConfiguration);

            var gatewaysConfiguration = new GatewaysSettings();
            this.Configuration.GetSection("Gateways").Bind(gatewaysConfiguration);

            services.AddSingleton<IRetryPolicyConfiguration>(retryPolicyConfiguration);
            services.AddSingleton<IGatewaysSettings>(gatewaysConfiguration);

            ConfigureSwagger(services);

            services
                .RegisterHttpClients()
                .RegisterMappers()
                .RegisterRepositories()
                .RegisterGateways()
                .RegisterServices();

            services.AddHttpClient(nameof(GatewaysSettings.TheDogApiGateway), c =>
            {
                c.BaseAddress = new Uri(gatewaysConfiguration.TheDogApiGateway.BaseUrl);
            });

            services.AddDbContext<DogShelterDbContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSingleton<IRetryService>(new RetryService(
                retryPolicyConfiguration.MaxRetry,
                TimeSpan.FromSeconds(retryPolicyConfiguration.PauseBetweenFailuresInSeconds)));
        }

        private static void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "DogShelterService", Version = "v1" });

                var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly);
                foreach (var xmlFile in xmlFiles)
                {
                    options.IncludeXmlComments(xmlFile);
                }

                options.UseInlineDefinitionsForEnums();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(_ => _.MapControllers());

            ConfigureSwaggerUI(app);

            var dbContext = serviceProvider.GetRequiredService<DogShelterDbContext>();
            dbContext.Database.EnsureCreated();
        }

        private static void ConfigureSwaggerUI(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DogShelterService API V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}