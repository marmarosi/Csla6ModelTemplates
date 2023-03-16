using Csla6ModelTemplates.Contracts.Complex.Edit;
using Csla6ModelTemplates.Models.Complex.Edit;
using Csla6ModelTemplates.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.WebApiTests.Complex
{
    public class Team_Tests
    {
        #region New

        [Fact]
        public async Task NewTeam_ReturnsNewModel()
        {
            // Arrange
            TestSetup setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<ComplexController>();
            var sut = new ComplexController(logger, setup.PortalFactory, setup.ChildPortalFactory);

            // Act
            ActionResult<TeamDto> actionResult = await sut.GetNewTeam();

            // Assert
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            TeamDto team = okObjectResult.Value as TeamDto;
            Assert.NotNull(team);

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
            // Arrange
            TestSetup setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<ComplexController>();
            var sut = new ComplexController(logger, setup.PortalFactory, setup.ChildPortalFactory);

            // Act
            TeamDto pristineTeam = null;
            PlayerDto pristinePlayer1 = null;
            PlayerDto pristinePlayer2 = null;

            pristineTeam = new TeamDto
            {
                TeamId = null,
                TeamCode = "T-9201",
                TeamName = "Test team number 9201",
                Timestamp = null
            };
            pristinePlayer1 = new PlayerDto
            {
                PlayerId = null,
                TeamId = null,
                PlayerCode = "P-9201-1",
                PlayerName = "Test player #1"
            };
            pristineTeam.Players.Add(pristinePlayer1);
            pristinePlayer2 = new PlayerDto
            {
                PlayerId = null,
                TeamId = null,
                PlayerCode = "P-9201-2",
                PlayerName = "Test player #2"
            };
            pristineTeam.Players.Add(pristinePlayer2);

            var actionResult = await sut.CreateTeam(pristineTeam);

            // Assert
            CreatedResult createdResult = actionResult.Result as CreatedResult;
            Assert.NotNull(createdResult);

            TeamDto createdTeam = createdResult.Value as TeamDto;
            Assert.NotNull(createdTeam);

            // The team must have new values.
            Assert.NotNull(createdTeam.TeamId);
            Assert.Equal(pristineTeam.TeamCode, createdTeam.TeamCode);
            Assert.Equal(pristineTeam.TeamName, createdTeam.TeamName);
            Assert.NotNull(createdTeam.Timestamp);

            // The players must have new values.
            Assert.Equal(2, createdTeam.Players.Count);

            PlayerDto createdPlayer1 = createdTeam.Players[0];
            Assert.NotNull(createdPlayer1.PlayerId);
            Assert.Equal(createdTeam.TeamId, createdPlayer1.TeamId);
            Assert.Equal(pristinePlayer1.PlayerCode, createdPlayer1.PlayerCode);
            Assert.Equal(pristinePlayer1.PlayerName, createdPlayer1.PlayerName);

            PlayerDto createdPlayer2 = createdTeam.Players[1];
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
            // Arrange
            TestSetup setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<ComplexController>();
            var sut = new ComplexController(logger, setup.PortalFactory, setup.ChildPortalFactory);

            // Act
            ActionResult<TeamDto> actionResult = await sut.GetTeam("LBgyGEK0PN2");

            // Assert
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            TeamDto pristineTeam = okObjectResult.Value as TeamDto;
            Assert.NotNull(pristineTeam);

            // The team code and name must end with 26.
            Assert.Equal("LBgyGEK0PN2", pristineTeam.TeamId);
            Assert.Equal("T-0026", pristineTeam.TeamCode);
            Assert.EndsWith("26", pristineTeam.TeamName);
            Assert.NotNull(pristineTeam.Timestamp);

            // The player codes and names must contain 26.
            Assert.True(pristineTeam.Players.Count > 0);
            foreach (PlayerDto player in pristineTeam.Players)
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
            // Arrange
            TestSetup setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<ComplexController>();
            var sutR = new ComplexController(logger, setup.PortalFactory, setup.ChildPortalFactory);
            var sutU = new ComplexController(logger, setup.PortalFactory, setup.ChildPortalFactory);

            // --- Act
            PlayerDto pristinePlayerNew = null;
            TeamDto pristineTeam = null;
            PlayerDto pristinePlayer1 = null;

            ActionResult<TeamDto> actionResultR = await sutR.GetTeam("JZY3GdKxyOj");
            OkObjectResult okObjectResultR = actionResultR.Result as OkObjectResult;
            pristineTeam = okObjectResultR.Value as TeamDto;
            pristinePlayer1 = pristineTeam.Players[0];

            pristineTeam.TeamCode = "T-9202";
            pristineTeam.TeamName = "Test team number 9202";
            pristinePlayer1.PlayerCode = "P-9202-1";
            pristinePlayer1.PlayerName = "Test player #9202.1";

            pristinePlayerNew = new PlayerDto
            {
                PlayerId = null,
                TeamId = null,
                PlayerCode = "P-9202-X",
                PlayerName = "Test player #9202.X"
            };
            pristineTeam.Players.Add(pristinePlayerNew);
            var actionResultU = await sutU.UpdateTeam(pristineTeam);

            // Assert
            OkObjectResult okObjectResultU = actionResultU.Result as OkObjectResult;
            Assert.NotNull(okObjectResultU);

            TeamDto updatedTeam = okObjectResultU.Value as TeamDto;
            Assert.NotNull(updatedTeam);

            // The team must have new values.
            Assert.Equal(pristineTeam.TeamId, updatedTeam.TeamId);
            Assert.Equal(pristineTeam.TeamCode, updatedTeam.TeamCode);
            Assert.Equal(pristineTeam.TeamName, updatedTeam.TeamName);
            Assert.NotEqual(pristineTeam.Timestamp, updatedTeam.Timestamp);

            Assert.Equal(pristineTeam.Players.Count, updatedTeam.Players.Count);

            // Players must reflect the changes.
            PlayerDto updatedPlayer1 = updatedTeam.Players[0];
            Assert.Equal(pristinePlayer1.PlayerCode, updatedPlayer1.PlayerCode);
            Assert.Equal(pristinePlayer1.PlayerName, updatedPlayer1.PlayerName);

            PlayerDto createdPlayerNew = updatedTeam.Players[pristineTeam.Players.Count - 1];
            Assert.Equal(pristinePlayerNew.PlayerCode, createdPlayerNew.PlayerCode);
            Assert.Equal(pristinePlayerNew.PlayerName, createdPlayerNew.PlayerName);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task DeleteTeam_ReturnsNothing()
        {
            // Arrange
            TestSetup setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<ComplexController>();
            var sut = new ComplexController(logger, setup.PortalFactory, setup.ChildPortalFactory);

            // Act
            ActionResult actionResult = await sut.DeleteTeam("qNwO0mkG3rB");

            // Assert
            NoContentResult noContentResult = actionResult as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        #endregion
    }
}
