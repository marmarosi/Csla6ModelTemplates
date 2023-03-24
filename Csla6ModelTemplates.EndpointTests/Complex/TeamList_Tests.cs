using Csla6ModelTemplates.Contracts.Complex.List;
using Csla6ModelTemplates.Endpoints.Complex;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.EndpointTests.Complex
{
    public class TeamList_Tests
    {
        [Fact]
        public async Task GetTeamList_ReturnsAList()
        {
            // Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<List>();
            var sut = new List(logger, setup.Csla);

            // Act
            var actionResult = await sut.HandleAsync(
                new TeamListCriteria { TeamName = "6" }
                );

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult);
            var list = Assert.IsAssignableFrom<IList<TeamListItemDto>>(okObjectResult.Value);

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
