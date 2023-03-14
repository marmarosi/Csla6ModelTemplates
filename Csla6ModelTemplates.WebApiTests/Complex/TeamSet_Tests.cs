using Csla6ModelTemplates.Contracts.Complex.Set;
using Csla6ModelTemplates.Models.Complex.Set;
using Csla6ModelTemplates.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.WebApiTests.Complex
{
    public class TeamSet_Tests
    {
        #region Read

        [Fact]
        public async Task ReadTeamSet_ReturnsCurrentModels()
        {
            // Arrange
            TestSetup setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<ComplexController>();
            var sut = new ComplexController(logger);

            // Act
            TeamSetCriteria criteria = new TeamSetCriteria { TeamName = "7" };
            ActionResult<List<TeamSetItemDto>> actionResult = await sut.GetTeamSet(
                criteria,
                setup.GetPortal<TeamSet>()
                );

            // Assert
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            List<TeamSetItemDto> pristineList = okObjectResult.Value as List<TeamSetItemDto>;
            Assert.NotNull(pristineList);

            // List must contain some items.
            Assert.Equal(5, pristineList.Count);
            foreach (TeamSetItemDto item in pristineList)
            {
                Assert.True(item.Players.Count > 0);
            }
        }

        #endregion

        #region Update

        [Fact]
        public async Task UpdateTeamSet_ReturnsUpdatedModels()
        {
            // Arrange
            TestSetup setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<ComplexController>();
            var sut = new ComplexController(logger);

            // Act
            TeamSetItemDto pristineTeam3 = null;
            TeamSetPlayerDto pristinePlayer31 = null;
            TeamSetItemDto pristineTeamNew = null;
            TeamSetPlayerDto pristinePlayerNew = null;
            string deletedTeamId = null;
            ActionResult<List<TeamSetItemDto>> actionResult = await Call<List<TeamSetItemDto>>.RetryOnDeadlock(async () =>
            {
                TeamSetCriteria criteria = new TeamSetCriteria { TeamName = "7" };
                ActionResult<List<TeamSetItemDto>> actionResult = await sut.GetTeamSet(
                    criteria,
                    setup.GetPortal<TeamSet>()
                    );
                OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
                List<TeamSetItemDto> pristineList = okObjectResult.Value as List<TeamSetItemDto>;

                // Modify an item.
                pristineTeam3 = pristineList[2];
                pristineTeam3.TeamCode = "T-9301";
                pristineTeam3.TeamName = "Test team number 9301";

                pristinePlayer31 = pristineTeam3.Players[0];
                pristinePlayer31.PlayerCode = "P-9301-1";
                pristinePlayer31.PlayerName = "Test player #9301.1";

                // Create new item.
                pristineTeamNew = new TeamSetItemDto
                {
                    TeamId = null,
                    TeamCode = "T-9302",
                    TeamName = "Test team number 9302",
                    Timestamp = null
                };
                pristinePlayerNew = new TeamSetPlayerDto
                {
                    PlayerId = null,
                    TeamId = null,
                    PlayerCode = "P-9302-X",
                    PlayerName = "Test player #9302.X"
                };
                pristineTeamNew.Players.Add(pristinePlayerNew);
                pristineList.Add(pristineTeamNew);

                // Delete an item.
                TeamSetItemDto pristineTeam4 = pristineList[3];
                deletedTeamId = pristineTeam4.TeamId;
                pristineList.Remove(pristineTeam4);

                // Act
                return await sut.UpdateTeamSet(
                    criteria,
                    pristineList,
                    setup.GetPortal<TeamSet>(),
                    setup.GetPortal<TeamSetItem>()
                    );
            });

            // Assert
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            List<TeamSetItemDto> updatedList = okObjectResult.Value as List<TeamSetItemDto>;
            Assert.NotNull(updatedList);

            // The updated team must have new values.
            TeamSetItemDto updatedTeam3 = updatedList[2];

            Assert.Equal(pristineTeam3.TeamId, updatedTeam3.TeamId);
            Assert.Equal(pristineTeam3.TeamCode, updatedTeam3.TeamCode);
            Assert.Equal(pristineTeam3.TeamName, updatedTeam3.TeamName);
            Assert.NotEqual(pristineTeam3.Timestamp, updatedTeam3.Timestamp);

            Assert.Equal(pristineTeam3.Players.Count, updatedTeam3.Players.Count);

            // The updated player must reflect the changes.
            TeamSetPlayerDto updatedPlayer31 = updatedTeam3.Players[0];
            Assert.Equal(pristinePlayer31.PlayerCode, updatedPlayer31.PlayerCode);
            Assert.Equal(pristinePlayer31.PlayerName, updatedPlayer31.PlayerName);

            // The created team must have new values.
            TeamSetItemDto createdTeam = updatedList
                .FirstOrDefault(o => o.TeamCode == pristineTeamNew.TeamCode);
            Assert.NotNull(createdTeam);

            Assert.NotNull(createdTeam.TeamId);
            Assert.Equal(pristineTeamNew.TeamCode, createdTeam.TeamCode);
            Assert.Equal(pristineTeamNew.TeamName, createdTeam.TeamName);
            Assert.NotNull(createdTeam.Timestamp);

            // The created team must have one player.
            Assert.Single(createdTeam.Players);

            TeamSetPlayerDto createdPlayer = createdTeam.Players[0];
            Assert.Equal(pristinePlayerNew.PlayerCode, createdPlayer.PlayerCode);
            Assert.Equal(pristinePlayerNew.PlayerName, createdPlayer.PlayerName);

            // The deleted team must have gone.
            TeamSetItemDto deleted = updatedList
                .FirstOrDefault(o => o.TeamId == deletedTeamId);
            Assert.Null(deleted);
        }

        #endregion
    }
}
