using Csla6ModelTemplates.Contracts.Simple.View;
using Csla6ModelTemplates.Endpoints.Simple;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.EndpointTests.Simple
{
    public class SimpleTeamView_Tests
    {
        [Fact]
        public async Task GetTeamView_ReturnsAView()
        {
            // Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<View>();
            var sut = new View(logger, setup.PortalFactory);

            // Act
            ActionResult<SimpleTeamViewDto> actionResult = await sut.HandleAsync("d9A30RLG8pZ");

            // Assert
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var team = okObjectResult.Value as SimpleTeamViewDto;
            Assert.NotNull(team);

            // The code and name must end with 31.
            Assert.Equal("T-0031", team.TeamCode);
            Assert.EndsWith("31", team.TeamName);
        }
    }
}
