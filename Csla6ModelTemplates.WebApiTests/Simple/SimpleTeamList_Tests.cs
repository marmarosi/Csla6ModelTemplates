using Csla;
using Csla6ModelTemplates.Contracts.Simple.List;
using Csla6ModelTemplates.Models.Simple.List;
using Csla6ModelTemplates.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        //public SimpleTeamList_Tests(
        //    ITestOutputHelper output,
        //    IDataPortal<SimpleTeamList> portal
        //    )
        //{
        //    this.output = output;
        //    this.portal = portal;
        //    app = new TestApplication();
        //}

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
            ActionResult<List<SimpleTeamListItemDto>> actionResult = await sut.GetTeamList(
                criteria, setup.GetPortal<SimpleTeamList>());

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
