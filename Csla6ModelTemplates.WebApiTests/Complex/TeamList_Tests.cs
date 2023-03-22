using Csla6ModelTemplates.Contracts.Complex.List;
using Csla6ModelTemplates.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.WebApiTests.Complex
{
    public class TeamList_Tests
    {
        [Fact]
        public async Task GetTeamList_ReturnsAList()
        {
            // Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<ComplexController>();
            var sut = new ComplexController(logger, setup.Csla);

            // Act
            TeamListCriteria criteria = new TeamListCriteria { TeamName = "6" };
            ActionResult<List<TeamListItemDto>> actionResult = await sut.GetTeamList(criteria);

            // Assert
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var list = okObjectResult.Value as List<TeamListItemDto>;
            Assert.NotNull(list);

            // The choice must have 5 items.
            Assert.Equal(5, list.Count);

            // The team code and names must end with 6.
            foreach (var team in list)
            {
                Assert.EndsWith("6", team.TeamCode);
                Assert.EndsWith("6", team.TeamName);
                Assert.True(team.Players.Count > 0);

                // The player code and names must contain 6.
                var player = team.Players[0];
                Assert.Contains("6", player.PlayerCode);
                Assert.Contains("6.", player.PlayerName);
            }
        }
    }
}
