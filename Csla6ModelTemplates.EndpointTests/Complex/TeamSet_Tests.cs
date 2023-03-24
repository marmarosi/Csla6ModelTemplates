using Csla6ModelTemplates.Contracts.Complex.Set;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Endpoints.Complex;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.EndpointTests.Complex
{
    public class TeamSet_Tests : TestBase
    {
        #region Read

        [Fact]
        public async Task ReadTeamSet_ReturnsCurrentModels()
        {
            // Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<ReadSet>();
            var sut = new ReadSet(logger, setup.Csla);

            // Act
            var actionResult = await sut.HandleAsync(
                new TeamSetCriteria { TeamName = "7" }
                );

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult);
            var pristineList = Assert.IsAssignableFrom<IList<TeamSetItemDto>>(okObjectResult.Value);

            // List must contain 5 items.
            Assert.Equal(5, pristineList.Count);
            foreach (var item in pristineList)
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
            var setup = TestSetup.GetInstance();
            var loggerRead = setup.GetLogger<ReadSet>();
            var sutRead = new ReadSet(loggerRead, setup.Csla);
            var loggerUpdate = setup.GetLogger<UpdateSet>();
            var sutUpdate = new UpdateSet(loggerUpdate, setup.Csla);

            // Act
            var criteria = new TeamSetCriteria { TeamName = "7" };
            var actionResultR = await sutRead.HandleAsync(criteria);
            var okObjectResultR = Assert.IsType<OkObjectResult>(actionResultR);
            var pristineList = Assert.IsAssignableFrom<IList<TeamSetItemDto>>(okObjectResultR.Value);

            // Modify an item.
            var pristineTeam3 = pristineList[2];
            pristineTeam3.TeamCode = "T-9301";
            pristineTeam3.TeamName = "Test team number 9301";

            var pristinePlayer31 = pristineTeam3.Players[0];
            pristinePlayer31.PlayerCode = "P-9301-1";
            pristinePlayer31.PlayerName = "Test player #9301.1";

            // Create new item.
            var pristineTeamNew = new TeamSetItemDto
            {
                TeamId = null,
                TeamCode = "T-9302",
                TeamName = "Test team number 9302",
                Timestamp = null
            };
            var pristinePlayerNew = new TeamSetPlayerDto
            {
                PlayerId = null,
                TeamId = null,
                PlayerCode = "P-9302-X",
                PlayerName = "Test player #9302.X"
            };
            pristineTeamNew.Players.Add(pristinePlayerNew);
            pristineList.Add(pristineTeamNew);

            // Delete an item.
            var pristineTeam4 = pristineList[3];
            var deletedTeamId = pristineTeam4.TeamId;
            pristineList.Remove(pristineTeam4);

            // Update now.
            TeamSetRequest request = new TeamSetRequest(criteria, (List<TeamSetItemDto>)pristineList);
            var actionResultU = await sutUpdate.HandleAsync(request, new CancellationToken());

            // Assert
            if (IsDeadlock(actionResultU, "TeamSet - Update")) return;
            var okObjectResultU = Assert.IsType<OkObjectResult>(actionResultU);
            var updatedList = Assert.IsAssignableFrom<IList<TeamSetItemDto>>(okObjectResultU.Value);

            // The updated team must have new values.
            var updatedTeam3 = updatedList[2];

            Assert.Equal(pristineTeam3.TeamId, updatedTeam3.TeamId);
            Assert.Equal(pristineTeam3.TeamCode, updatedTeam3.TeamCode);
            Assert.Equal(pristineTeam3.TeamName, updatedTeam3.TeamName);
            Assert.NotEqual(pristineTeam3.Timestamp, updatedTeam3.Timestamp);

            Assert.Equal(pristineTeam3.Players.Count, updatedTeam3.Players.Count);

            // The updated player must reflect the changes.
            var updatedPlayer31 = updatedTeam3.Players[0];
            Assert.Equal(pristinePlayer31.PlayerCode, updatedPlayer31.PlayerCode);
            Assert.Equal(pristinePlayer31.PlayerName, updatedPlayer31.PlayerName);

            // The created team must have new values.
            var createdTeam = updatedList
                .FirstOrDefault(o => o.TeamCode == pristineTeamNew.TeamCode);
            Assert.NotNull(createdTeam);

            Assert.NotNull(createdTeam.TeamId);
            Assert.Equal(pristineTeamNew.TeamCode, createdTeam.TeamCode);
            Assert.Equal(pristineTeamNew.TeamName, createdTeam.TeamName);
            Assert.NotNull(createdTeam.Timestamp);

            // The created team must have one player.
            Assert.Single(createdTeam.Players);

            var createdPlayer = createdTeam.Players[0];
            Assert.Equal(pristinePlayerNew.PlayerCode, createdPlayer.PlayerCode);
            Assert.Equal(pristinePlayerNew.PlayerName, createdPlayer.PlayerName);

            // The deleted team must have gone.
            var deleted = updatedList
                .FirstOrDefault(o => o.TeamId == deletedTeamId);
            Assert.Null(deleted);
        }

        #endregion
    }
}
