﻿using Csla6ModelTemplates.Dal.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Csla6ModelTemplates.Configuration
{
    /// <summary>
    /// Configuration extension methods
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Add the services for ProjectTracker.Dal that
        /// use Entity Framework
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">Teh application configuration.</param>
        public static void AddSqlServerDal(
            this IServiceCollection services,
            IConfiguration configuration = null
            )
        {
            // Configure database.
            if (configuration == null)
                services.AddDbContext<SqlServerContext>(options => options
                    .UseSqlServer("name=ConnectionStrings:SQLServer")
                    );
            else
                services.AddDbContext<SqlServerContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("SQLServer"))
                );

            // Configure data access layer.
            foreach (var dalType in SqlServerDalIndex.Items)
                services.AddTransient(dalType.Key, dalType.Value);
        }

        /// <summary>
        /// Runs seeders of persistent storages.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="isDevelopment">Indicates whether the hosting environment is development..</param>
        /// <param name="contentRootPath">The root path of the web site.</param>
        public static void RunSeeders(
            this IApplicationBuilder app,
            bool isDevelopment,
            string contentRootPath
            )
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<SqlServerContext>();

                SqlServerSeeder.Run(context, isDevelopment, contentRootPath);
            }
        }
    }
}