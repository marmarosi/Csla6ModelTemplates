using Csla6ModelTemplates.Contracts.Simple.Command;
using Csla6ModelTemplates.CslaExtensions.Utilities;
using Csla6ModelTemplates.Endpoints.Simple;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.EndpointTests.Simple
{
    public class RenameTeam_Tests : TestBase
    {
        [Fact]
        public async Task RenameTeam_ReturnsTrue()
        {
            // ********** Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<Command>();
            var sut = new Command(logger, setup.Csla);

            // ********** Act
            var dto = new RenameTeamDto
            {
                TeamId = "oZkzGJ6G794",
                TeamName = "Team Thirty Seven"
            };
            var actionResult = await sut.HandleAsync(dto);

            // ********** Assert
            if (IsDeadlock(actionResult, "RenameTeam - Execute")) return;
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult);
            var success = Assert.IsAssignableFrom<bool>(okObjectResult.Value);

            Assert.True(success);
        }
    }
}
