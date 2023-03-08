using Csla6ModelTemplates.Contracts.Simple.View;
using Csla6ModelTemplates.Models.Simple.View;
using Csla6ModelTemplates.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.WebApiTests.Simple
{
    public class SimpleTeamView_Tests
    {
        [Fact]
        public async Task GetTeamView_ReturnsAView()
        {
            // Arrange
            TestSetup setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<SimpleController>();
            var sut = new SimpleController(logger);

            // Act
            ActionResult<SimpleTeamViewDto> actionResult = await sut.GetTeamView(
                "d9A30RLG8pZ",
                setup.GetPortal<SimpleTeamView>()
                );

            // Assert
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            SimpleTeamViewDto team = okObjectResult.Value as SimpleTeamViewDto;
            Assert.NotNull(team);

            // The code and name must end with 31.
            Assert.Equal("T-0031", team.TeamCode);
            Assert.EndsWith("31", team.TeamName);
        }
    }
}
