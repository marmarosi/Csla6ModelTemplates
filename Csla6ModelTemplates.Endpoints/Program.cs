using Csla.Configuration;
using Csla6ModelTemplates.Configuration;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Dal;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((ctx, configBuilder) =>
{
    var enviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    var webRootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    var basePath = Path.Join(webRootPath, "../../..");
    var sharedSettings = Path.Join(basePath, "../Shared/SharedSettings.json");

    //configBuilder.SetBasePath(basePath);
    configBuilder.AddJsonFile(sharedSettings, false, true);
    configBuilder.AddJsonFile("appsettings.json", false, true);
    configBuilder.AddJsonFile($"appsettings.{enviroment}.json", true, true);
    //configBuilder.AddJsonFile($"appsettings.{Environment.MachineName}.json", true, true);

    configBuilder.AddEnvironmentVariables();
});

// ********** Add services to the container.

builder.Services.AddCors(options => {
    options.AddPolicy(
        "Csla6ModelTemplatesPolicy",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
        );
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
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

// Configure data access layer.
IDeadLockDetector detector = new DeadLockDetector();
builder.Services.AddSingleton(detector);
builder.Services.AddMySqlDal(detector);
//builder.Services.AddPostgreSqlDal(detector);
//builder.Services.AddSqlServerDal(detector);
builder.Services.AddSingleton(typeof(ITransactionOptions), new TransactionOptions(false));

// If using Kestrel:
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});
// If using IIS:
builder.Services.Configure<IISServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

// Configure CSLA.
builder.Services.AddHttpContextAccessor();
builder.Services.AddCsla(o => o
    .AddAspNetCore()
    .DataPortal(dpo => dpo
        .UseLocalProxy(options => {
            options.UseLocalScope = true;
            options.FlowSynchronizationContext = false;
        })
    )
);
builder.Services.AddScoped<ICslaService, CslaService>();

builder.Services.AddControllers();

// ********** Add middlewares to the request life cycle.

var app = builder.Build();

app.RunMySqlSeeders(app.Environment.IsDevelopment(), app.Environment.ContentRootPath);
//app.RunPostgreSqlSeeders(app.Environment.IsDevelopment(), app.Environment.ContentRootPath);
//app.RunSqlServerSeeders(app.Environment.IsDevelopment(), app.Environment.ContentRootPath);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o => o.DocExpansion(DocExpansion.None));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
