using Csla6ModelTemplates.Contracts.Simple.Command;
using Csla6ModelTemplates.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.WebApiTests.Simple
{
    public class RenameTeam_Tests
    {
        [Fact]
        public async Task RenameTeam_ReturnsTrue()
        {
            // Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<SimpleController>();
            var sut = new SimpleController(logger, setup.Csla);

            // Act
            var dto = new RenameTeamDto
            {
                TeamId = "oZkzGJ6G794",
                TeamName = "Team Thirty Seven"
            };
            ActionResult<bool> actionResult = await sut.RenameTeamCommand(dto);

            // Assert
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            bool success = (bool)okObjectResult.Value;
            Assert.True(success);
        }
    }
}
