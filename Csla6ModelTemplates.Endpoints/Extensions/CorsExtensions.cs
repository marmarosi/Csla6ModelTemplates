using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Csla6ModelTemplates.Endpoints.Extensions
{
    /// <summary>
    /// Provide methods to configure CORS.
    /// </summary>
    internal static class CorsExtensions
    {
        /// <summary>
        /// Sets up cross origin resource sharing servcies.
        /// </summary>
        /// <param name="options">Provides prorammatic configuration for CORS.</param>
        public static void Setup(
            CorsOptions options
            )
        {
            options.AddPolicy(
                "Csla6ModelTemplatesPolicy",
                builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                );
        }
    }
}
