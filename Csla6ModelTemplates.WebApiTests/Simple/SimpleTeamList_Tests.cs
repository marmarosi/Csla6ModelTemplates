using Csla;
using Csla.Configuration;
using Csla6ModelTemplates.Contracts.Simple.List;
using Csla6ModelTemplates.Dal.SqlServer;
using Csla6ModelTemplates.Dal.SqlServer.Simple.List;
using Csla6ModelTemplates.Models.Simple.List;
using Csla6ModelTemplates.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Csla6ModelTemplates.WebApiTests.Simple
{
    public class SimpleTeamList_Tests
    {
        //private readonly ITestOutputHelper output;
        //private readonly IDataPortal<SimpleTeamList> portal;
        //private readonly TestApplication app;
        private readonly ApplicationContext appContext;

        public SimpleTeamList_Tests(
            //ITestOutputHelper output,
            //IDataPortal<SimpleTeamList> portal
            )
        {
            //this.output = output;
            //this.portal = portal;
            //app = new TestApplication();

            var services = new ServiceCollection();
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("SharedSettings.json", true, true)
                .AddJsonFile("appsettings.json", true, true) // use appsettings.json in current folder
                .AddEnvironmentVariables();
            IConfiguration configuration = builder.Build();
            services.AddDbContext<SqlServerContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SQLServer"))
            );
            services.AddCsla();
            services.AddScoped<ISimpleTeamListDal, SimpleTeamListDal>();
            var sp = services.BuildServiceProvider();
            appContext = sp.GetRequiredService<ApplicationContext>();
        }

        [Fact]
        public async Task GetTeamList_ReturnsAList()
        {
            ////var client = app.CreateDefaultClient();
            ////var result = await client.GetStringAsync("/");
            //var logger = app.GetLogger<SimpleController>();
            //var sut = new SimpleController(logger);

            // Arrange
            SetupService setup = SetupService.GetInstance();
            var logger = setup.GetLogger<SimpleController>();
            var sut = new SimpleController(logger);

            // Act
            SimpleTeamListCriteria criteria = new SimpleTeamListCriteria { TeamName = "9" };
            var portal = appContext.CreateInstanceDI<DataPortal<SimpleTeamList>>();
            ActionResult<List<SimpleTeamListItemDto>> actionResult = await sut.GetTeamList(
                criteria, portal);

            // Assert
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            List<SimpleTeamListItemDto> list = okObjectResult.Value as List<SimpleTeamListItemDto>;
            Assert.NotNull(list);

            // The list must have 5 items.
            Assert.Equal(5, list.Count);

            // The code and names must end with 9.
            foreach (var item in list)
            {
                Assert.EndsWith("9", item.TeamCode);
                Assert.EndsWith("9", item.TeamName);
            }
        }
    }
}
