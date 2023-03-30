using Csla6ModelTemplates.Endpoints.Extensions;

// ---------- Create the app builder.
var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration(ConfigExtensions.Build);

// ********** Add services to the container.

builder.Services.AddCors(CorsExtensions.Setup);

builder.Services.AddSwaggerGenerator(builder.Environment);

builder.Services.AddDataAccessLayers();

builder.Services.AddCslaLibrary();

builder.Services.AddControllers();

// ********** Add middlewares to the request life cycle.

// ---------- Build the application.
var app = builder.Build();

app.RunStorageSeeders();

// ********** Configure the HTTP request pipeline.

app.UseSwaggerDocumentation();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// ---------- Start the application.
app.Run();
