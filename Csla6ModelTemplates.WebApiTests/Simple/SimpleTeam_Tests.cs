using Csla6ModelTemplates.Contracts.Simple.Edit;
using Csla6ModelTemplates.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.WebApiTests.Simple
{
    public class SimpleTeam_Tests
    {
        #region New

        [Fact]
        public async Task NewTeam_ReturnsNewModel()
        {
            // Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<SimpleController>();
            var sut = new SimpleController(logger, setup.Csla);

            // Act
            ActionResult<SimpleTeamDto> actionResult = await sut.GetNewTeam();

            // Assert
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var team = okObjectResult.Value as SimpleTeamDto;
            Assert.NotNull(team);

            // The code and name must miss.
            Assert.Empty(team.TeamCode);
            Assert.Empty(team.TeamName);
            Assert.Null(team.Timestamp);
        }

        #endregion

        #region Create

        [Fact]
        public async Task CreateTeam_ReturnsCreatedModel()
        {
            // Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<SimpleController>();
            var sut = new SimpleController(logger, setup.Csla);

            // Act
            var pristineTeam = new SimpleTeamDto
            {
                TeamId = null,
                TeamCode = "T-9001",
                TeamName = "Test team number 9001",
                Timestamp = null
            };
            ActionResult<SimpleTeamDto> actionResult = await sut.CreateTeam(pristineTeam);

            // Assert
            var createdResult = actionResult.Result as CreatedResult;
            Assert.NotNull(createdResult);

            var createdTeam = createdResult.Value as SimpleTeamDto;
            Assert.NotNull(createdTeam);

            // The model must have new values.
            Assert.NotNull(createdTeam.TeamId);
            Assert.Equal(pristineTeam.TeamCode, createdTeam.TeamCode);
            Assert.Equal(pristineTeam.TeamName, createdTeam.TeamName);
            Assert.NotNull(createdTeam.Timestamp);
        }

        #endregion

        #region Read

        [Fact]
        public async Task ReadTeam_ReturnsCurrentModel()
        {
            // Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<SimpleController>();
            var sut = new SimpleController(logger, setup.Csla);

            // Act
            ActionResult<SimpleTeamDto> actionResult = await sut.GetTeam("zXayGQW0bZv");

            // Assert
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var pristine = okObjectResult.Value as SimpleTeamDto;
            Assert.NotNull(pristine);

            // The code and name must end with 22.
            Assert.Equal("zXayGQW0bZv", pristine.TeamId);
            Assert.Equal("T-0022", pristine.TeamCode);
            Assert.EndsWith("22", pristine.TeamName);
            Assert.NotNull(pristine.Timestamp);
        }

        #endregion

        #region Update

        [Fact]
        public async Task UpdateTeam_ReturnsUpdatedModel()
        {
            // Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<SimpleController>();
            var sutR = new SimpleController(logger, setup.Csla);
            var sutU = new SimpleController(logger, setup.Csla);

            // Act
            ActionResult<SimpleTeamDto> actionResultR = await sutR.GetTeam("zXayGQW0bZv");
            var okObjectResultR = actionResultR.Result as OkObjectResult;
            var pristine = okObjectResultR.Value as SimpleTeamDto;

            pristine.TeamCode = "T-9002";
            pristine.TeamName = "Test team number 9002";

            ActionResult<SimpleTeamDto> actionResultU = await sutU.UpdateTeam(pristine);

            // Assert
            var okObjectResultU = actionResultU.Result as OkObjectResult;
            Assert.NotNull(okObjectResultU);

            var updated = okObjectResultU.Value as SimpleTeamDto;
            Assert.NotNull(updated);

            // The team must have new values.
            Assert.Equal(pristine.TeamId, updated.TeamId);
            Assert.Equal(pristine.TeamCode, updated.TeamCode);
            Assert.Equal(pristine.TeamName, updated.TeamName);
            Assert.NotEqual(pristine.Timestamp, updated.Timestamp);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task DeleteTeam_ReturnsNothing()
        {
            // Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<SimpleController>();
            var sut = new SimpleController(logger, setup.Csla);

            // Act
            ActionResult actionResult = await sut.DeleteTeam("rWqG7KpG5Qo");

            // Assert
            var noContentResult = actionResult as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        #endregion
    }
}
