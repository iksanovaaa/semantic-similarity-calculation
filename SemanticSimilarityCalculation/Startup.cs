using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SemanticSimilarityCalculation.Services;
using SemanticSimilarityCalculation.Services.Interfaces;
using System;
using Microsoft.OpenApi.Models;

namespace SemanticSimilarityCalculation
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
            services.AddControllers();
            services.AddScoped<IAnnotationService, AnnotationService>();
            services.AddScoped<ISimilarityService, CosineSimilarityService>();
            services.AddScoped<ITextProcessingService, TextProcessingService>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Semantic Similarity API",
                    Description = "A module for semantic similarity calculation API",
                    Contact = new OpenApiContact
                    {
                        Name = "Alena Iksanova",
                        Email = "iksanovaal@yandex.ru",
                        Url = new Uri("https://github.com/iksanovaaa"),
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Semantic Similarity API");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
