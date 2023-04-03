﻿using Csla.Configuration;
using Csla6ModelTemplates.CslaExtensions;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Csla6ModelTemplates.WebApi.Extensions
{
    /// <summary>
    /// Provide methods to configure CSLA.
    /// </summary>
    internal static class CslaExtensions
    {
        /// <summary>
        /// Configures the CSLA business object library.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void AddCslaLibrary(
            this IServiceCollection services
            )
        {
            // If using Kestrel:
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddHttpContextAccessor();

            services.AddCsla(o => o
                .AddAspNetCore()
                .DataPortal(dpo => dpo
                    .UseLocalProxy(options => {
                        options.UseLocalScope = true;
                        options.FlowSynchronizationContext = false;
                    })
                )
            );
            services.AddScoped<ICslaService, CslaService>();
        }
    }
}