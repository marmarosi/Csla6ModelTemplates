using Csla6ModelTemplates.Contracts.Simple.Edit;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Endpoints.Simple;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.EndpointTests.Simple
{
    public class SimpleTeam_Tests : TestBase
    {
        #region New

        [Fact]
        public async Task NewTeam_ReturnsNewModel()
        {
            // ********** Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<New>();
            var sut = new New(logger, setup.Csla);

            // ********** Act
            var actionResult = await sut.HandleAsync();

            // ********** Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult);
            var team = Assert.IsAssignableFrom<SimpleTeamDto>(okObjectResult.Value);

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
            // ********** Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<Create>();
            var sut = new Create(logger, setup.Csla);

            // ********** Act
            var pristineTeam = new SimpleTeamDto
            {
                TeamId = null,
                TeamCode = "T-9001",
                TeamName = "Test team number 9001",
                Timestamp = null
            };
            var actionResult = await sut.HandleAsync(pristineTeam);

            // ********** Assert
            if (IsDeadlock(actionResult, "SimpleTeam - Create")) return;
            var createdResult = Assert.IsType<CreatedResult>(actionResult);
            var createdTeam = Assert.IsAssignableFrom<SimpleTeamDto>(createdResult.Value);

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
            // ********** Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<Read>();
            var sut = new Read(logger, setup.Csla);

            // ********** Act
            var actionResult = await sut.HandleAsync("zXayGQW0bZv");

            // ********** Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult);
            var team = Assert.IsAssignableFrom<SimpleTeamDto>(okObjectResult.Value);

            // The code and name must end with 22.
            Assert.Equal("zXayGQW0bZv", team.TeamId);
            Assert.Equal("T-0022", team.TeamCode);
            Assert.EndsWith("22", team.TeamName);
            Assert.NotNull(team.Timestamp);
        }

        #endregion

        #region Update

        [Fact]
        public async Task UpdateTeam_ReturnsUpdatedModel()
        {
            // ********** Arrange
            var setup = TestSetup.GetInstance();
            var loggerR = setup.GetLogger<Read>();
            var loggerU = setup.GetLogger<Update>();
            var sutR = new Read(loggerR, setup.Csla);
            var sutU = new Update(loggerU, setup.Csla);

            // ********** Act
            var actionResultR = await sutR.HandleAsync("zXayGQW0bZv");
            var okObjectResultR = Assert.IsType<OkObjectResult>(actionResultR);
            var pristine = Assert.IsAssignableFrom<SimpleTeamDto>(okObjectResultR.Value);

            pristine.TeamCode = "T-9002";
            pristine.TeamName = "Test team number 9002";
            var actionResultU = await sutU.HandleAsync(pristine, new CancellationToken());

            // ********** Assert
            if (IsDeadlock(actionResultU, "SimpleTeam - Update")) return;
            var okObjectResultU = Assert.IsType<OkObjectResult>(actionResultU);
            var updated = Assert.IsAssignableFrom<SimpleTeamDto>(okObjectResultU.Value);

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
            // ********** Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<Delete>();
            var sut = new Delete(logger, setup.Csla);

            // ********** Act
            var actionResult = await sut.HandleAsync("rWqG7KpG5Qo");

            // ********** Assert
            if (IsDeadlock(actionResult, "SimpleTeam - Delete")) return;
            var noContentResult = Assert.IsType<NoContentResult>(actionResult);

            Assert.Equal(204, noContentResult.StatusCode);
        }

        #endregion
    }
}
