using Csla6ModelTemplates.Contracts.Complex.View;
using Csla6ModelTemplates.Models.Complex.View;
using Csla6ModelTemplates.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.WebApiTests.Complex
{
    public class TeamView_Tests
    {
        [Fact]
        public async Task GetTeamView_ReturnsAView()
        {
            // Arrange
            TestSetup setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<ComplexController>();
            var sut = new ComplexController(logger);

            // Act
            ActionResult<TeamViewDto> actionResult = await sut.GetTeamView(
                "1r9oGj1x3lk",
                setup.GetPortal<TeamView>()
                );

            // Assert
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            TeamViewDto team = okObjectResult.Value as TeamViewDto;
            Assert.NotNull(team);

            // The code and name must end with 17.
            Assert.Equal("T-0017", team.TeamCode);
            Assert.EndsWith("17", team.TeamName);
            Assert.True(team.Players.Count > 0);

            // The code and name must end with 17.
            PlayerViewDto player = team.Players[0];
            Assert.StartsWith("P-0017", player.PlayerCode);
            Assert.Contains("17.", player.PlayerName);
        }
    }
}
