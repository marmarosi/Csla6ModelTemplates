using Csla6ModelTemplates.Contracts.Complex.Command;
using Csla6ModelTemplates.Models.Complex.Command;
using Csla6ModelTemplates.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.WebApiTests.Complex
{
    public class CountTeams_Tests
    {
        [Fact]
        public async Task CountTeams_ReturnsList()
        {
            // Arrange
            TestSetup setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<ComplexController>();
            var sut = new ComplexController(logger);

            // Act
            ActionResult<List<CountTeamsResultDto>> actionResult = await sut.CountTeamsCommand(
                new CountTeamsCriteria(),
                setup.GetPortal<CountTeams>()
                );

            // Assert
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            List<CountTeamsResultDto> list = okObjectResult.Value as List<CountTeamsResultDto>;
            Assert.NotNull(list);

            // Count list must contain 4 items.
            Assert.Equal(4, list.Count);

            CountTeamsResultDto item1 = list[0];
            Assert.Equal(4, item1.ItemCount);
            Assert.True(item1.CountOfTeams > 0);

            CountTeamsResultDto item2 = list[1];
            Assert.Equal(3, item2.ItemCount);
            Assert.True(item2.CountOfTeams > 0);

            CountTeamsResultDto item3 = list[2];
            Assert.Equal(2, item3.ItemCount);
            Assert.True(item3.CountOfTeams > 0);

            CountTeamsResultDto item4 = list[3];
            Assert.Equal(1, item4.ItemCount);
            Assert.True(item4.CountOfTeams > 0);
        }
    }
}
