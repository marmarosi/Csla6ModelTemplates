using Csla6ModelTemplates.Contracts.Complex.Edit;
using Csla6ModelTemplates.CslaExtensions.Utilities;
using Csla6ModelTemplates.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.WebApiTests.Complex
{
    public class Team_Tests : TestBase
    {
        #region New

        [Fact]
        public async Task NewTeam_ReturnsNewModel()
        {
            // ********** Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<ComplexController>();
            var sut = new ComplexController(logger, setup.Csla);

            // ********** Act
            var actionResult = await sut.GetNewTeam();

            // ********** Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult);
            var team = Assert.IsAssignableFrom<TeamDto>(okObjectResult.Value);

            // The code and name must miss.
            Assert.Empty(team.TeamCode);
            Assert.Empty(team.TeamName);
            Assert.Null(team.Timestamp);
            Assert.Empty(team.Players);
        }

        #endregion

        #region Create

        [Fact]
        public async Task CreateTeam_ReturnsCreatedModel()
        {
            // ********** Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<ComplexController>();
            var sut = new ComplexController(logger, setup.Csla);

            // ********** Act
            var pristineTeam = new TeamDto
            {
                TeamId = null,
                TeamCode = "T-9201",
                TeamName = "Test team number 9201",
                Timestamp = null
            };
            var pristinePlayer1 = new TeamPlayerDto
            {
                PlayerId = null,
                TeamId = null,
                PlayerCode = "P-9201-1",
                PlayerName = "Test player #1"
            };
            pristineTeam.Players.Add(pristinePlayer1);
            var pristinePlayer2 = new TeamPlayerDto
            {
                PlayerId = null,
                TeamId = null,
                PlayerCode = "P-9201-2",
                PlayerName = "Test player #2"
            };
            pristineTeam.Players.Add(pristinePlayer2);

            var actionResult = await sut.CreateTeam(pristineTeam);

            // ********** Assert
            if (IsDeadlock(actionResult, "Team - Create")) return;
            var createdResult = Assert.IsType<CreatedResult>(actionResult);
            var createdTeam = Assert.IsAssignableFrom<TeamDto>(createdResult.Value);

            // The team must have new values.
            Assert.NotNull(createdTeam.TeamId);
            Assert.Equal(pristineTeam.TeamCode, createdTeam.TeamCode);
            Assert.Equal(pristineTeam.TeamName, createdTeam.TeamName);
            Assert.NotNull(createdTeam.Timestamp);

            // The players must have new values.
            Assert.Equal(2, createdTeam.Players.Count);

            var createdPlayer1 = createdTeam.Players[0];
            Assert.NotNull(createdPlayer1.PlayerId);
            Assert.Equal(createdTeam.TeamId, createdPlayer1.TeamId);
            Assert.Equal(pristinePlayer1.PlayerCode, createdPlayer1.PlayerCode);
            Assert.Equal(pristinePlayer1.PlayerName, createdPlayer1.PlayerName);

            var createdPlayer2 = createdTeam.Players[1];
            Assert.NotNull(createdPlayer2.PlayerId);
            Assert.Equal(createdTeam.TeamId, createdPlayer2.TeamId);
            Assert.Equal(pristinePlayer2.PlayerCode, createdPlayer2.PlayerCode);
            Assert.Equal(pristinePlayer2.PlayerName, createdPlayer2.PlayerName);
        }

        #endregion

        #region Read

        [Fact]
        public async Task ReadTeam_ReturnsCurrentModel()
        {
            // ********** Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<ComplexController>();
            var sut = new ComplexController(logger, setup.Csla);

            // ********** Act
            var actionResult = await sut.GetTeam("LBgyGEK0PN2");

            // ********** Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult);
            var team = Assert.IsAssignableFrom<TeamDto>(okObjectResult.Value);

            // The team code and name must end with 26.
            Assert.Equal("LBgyGEK0PN2", team.TeamId);
            Assert.Equal("T-0026", team.TeamCode);
            Assert.EndsWith("26", team.TeamName);
            Assert.NotNull(team.Timestamp);

            // The player codes and names must contain 26.
            Assert.True(team.Players.Count > 0);
            foreach (var player in team.Players)
            {
                Assert.Equal("LBgyGEK0PN2", player.TeamId);
                Assert.Contains("26", player.PlayerCode);
                Assert.Contains("26", player.PlayerName);
            }
        }

        #endregion

        #region Update

        [Fact]
        public async Task UpdateTeam_ReturnsUpdatedModel()
        {
            // ********** Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<ComplexController>();
            var sutR = new ComplexController(logger, setup.Csla);
            var sutU = new ComplexController(logger, setup.Csla);

            // --- Act
            var actionResultR = await sutR.GetTeam("JZY3GdKxyOj");
            var okObjectResultR = Assert.IsType<OkObjectResult>(actionResultR);
            var pristineTeam = Assert.IsAssignableFrom<TeamDto>(okObjectResultR.Value);
            var pristinePlayer1 = pristineTeam.Players[0];

            pristineTeam.TeamCode = "T-9202";
            pristineTeam.TeamName = "Test team number 9202";
            pristinePlayer1.PlayerCode = "P-9202-1";
            pristinePlayer1.PlayerName = "Test player #9202.1";

            var pristinePlayerNew = new TeamPlayerDto
            {
                PlayerId = null,
                TeamId = null,
                PlayerCode = "P-9202-X",
                PlayerName = "Test player #9202.X"
            };
            pristineTeam.Players.Add(pristinePlayerNew);
            var actionResultU = await sutU.UpdateTeam(pristineTeam);

            // ********** Assert
            if (IsDeadlock(actionResultU, "Team - Update")) return;
            var okObjectResultU = Assert.IsType<OkObjectResult>(actionResultU);
            var updatedTeam = Assert.IsAssignableFrom<TeamDto>(okObjectResultU.Value);

            // The team must have new values.
            Assert.Equal(pristineTeam.TeamId, updatedTeam.TeamId);
            Assert.Equal(pristineTeam.TeamCode, updatedTeam.TeamCode);
            Assert.Equal(pristineTeam.TeamName, updatedTeam.TeamName);
            Assert.NotEqual(pristineTeam.Timestamp, updatedTeam.Timestamp);

            Assert.Equal(pristineTeam.Players.Count, updatedTeam.Players.Count);

            // Players must reflect the changes.
            var updatedPlayer1 = updatedTeam.Players[0];
            Assert.Equal(pristinePlayer1.PlayerCode, updatedPlayer1.PlayerCode);
            Assert.Equal(pristinePlayer1.PlayerName, updatedPlayer1.PlayerName);

            var createdPlayerNew = updatedTeam.Players[pristineTeam.Players.Count - 1];
            Assert.Equal(pristinePlayerNew.PlayerCode, createdPlayerNew.PlayerCode);
            Assert.Equal(pristinePlayerNew.PlayerName, createdPlayerNew.PlayerName);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task DeleteTeam_ReturnsNothing()
        {
            // ********** Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<ComplexController>();
            var sut = new ComplexController(logger, setup.Csla);

            // ********** Act
            var actionResult = await sut.DeleteTeam("qNwO0mkG3rB");

            // ********** Assert
            if (IsDeadlock(actionResult, "Team - Delete")) return;
            var noContentResult = Assert.IsType<NoContentResult>(actionResult);

            Assert.Equal(204, noContentResult.StatusCode);
        }

        #endregion
    }
}
