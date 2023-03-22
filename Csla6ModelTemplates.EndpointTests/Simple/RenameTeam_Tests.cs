using Csla6ModelTemplates.Contracts.Simple.Command;
using Csla6ModelTemplates.Endpoints.Simple;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.EndpointTests.Simple
{
    public class RenameTeam_Tests
    {
        [Fact]
        public async Task RenameTeam_ReturnsTrue()
        {
            // Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<Command>();
            var sut = new Command(logger, setup.Csla);

            // Act
            var dto = new RenameTeamDto
            {
                TeamId = "oZkzGJ6G794",
                TeamName = "Team Thirty Seven"
            };
            ActionResult<bool> actionResult = await sut.HandleAsync(dto);

            // Assert
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            bool success = (bool)okObjectResult.Value;
            Assert.True(success);
        }
    }
}
