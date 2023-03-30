using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

namespace Csla6ModelTemplates.Endpoints.Extensions
{
    /// <summary>
    /// Provide methods to configure Swagger.
    /// </summary>
    internal static class SwaggerExtensions
    {
        /// <summary>
        /// Configures the Swagger generator.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="environment">The web hosting environment the application is running in.</param>
        public static void AddSwaggerGenerator(
            this IServiceCollection services,
            IWebHostEnvironment environment
            )
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "CSLA 6 REST API",
                        Description = string.Format("CSLA 6 model templates used in REST API ● Version {0}",
                            Assembly
                                .GetEntryAssembly()
                                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                                .InformationalVersion
                        )
                    }
                );
                o.EnableAnnotations();
            });
        }

        /// <summary>
        /// Registers the Swagger middlewares.
        /// </summary>
        /// <param name="app">The web application.</param>
        public static void UseSwaggerDocumentation(
            this WebApplication app
            )
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(o => o.DocExpansion(DocExpansion.None));
            }
        }
    }
}
