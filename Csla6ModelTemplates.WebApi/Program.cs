using Csla.Configuration;
using Csla6ModelTemplates.Configuration;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options => {
    options.AddPolicy(
        "Csla6ModelTemplatesPolicy",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
        );
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    string xmlFile = $"{builder.Environment.ApplicationName}.xml";
    string xmlPath = Path.Combine(builder.Environment.ContentRootPath, xmlFile);
    o.IncludeXmlComments(xmlPath, true);
});

// Configure data access layer.
builder.Services.AddSqlServerDal();

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

var app = builder.Build();

app.RunSeeders(app.Environment.IsDevelopment(), app.Environment.ContentRootPath);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    /* ----------------------------------- //

    app.MapGet(
        "/endpoint/simple/{teamId}",
        [SwaggerOperation(
            Summary = "Gets the specified team details to display.",
            Description = "Gets the specified team details to display.<br>" +
                "Criteria:<br>{<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;teamId: string<br>" +
                "}<br>" +
                "Result: SimpleTeamViewDto",
            OperationId = "SimpleTeam.View",
            Tags = new[] { "Simple Endpoints" })]
        async (string teamId, IDataPortal<SimpleTeamView> portal) =>
        {
            var criteria = new SimpleTeamViewParams { TeamId = teamId };
            var team = await portal.FetchAsync(criteria.Decode());
            return Results.Ok(team);
        })
    //.WithGroupName("Team")
    //.WithName("Get simple team view")
    .Produces<SimpleTeamView>(contentType: "application/json")
    //.WithDisplayName("Display name")
    //.WithMetadata(new { Summary: "Summary"})
    .WithTags("Simple Endpoints")
    //.WithSummary("")
    ;
    // ----------------------------------- */
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
