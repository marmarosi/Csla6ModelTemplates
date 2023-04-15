using Csla6ModelTemplates.Contracts.Complex.Command;
using Csla6ModelTemplates.Endpoints.Complex;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.EndpointTests.Complex
{
    public class CountTeams_Tests
    {
        [Fact]
        public async Task CountTeams_ReturnsList()
        {
            // ********** Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<Command>();
            var sut = new Command(logger, setup.Csla);

            // ********** Act
            var actionResult = await sut.HandleAsync(
                new CountTeamsCriteria()
                );

            // ********** Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult);
            var list = Assert.IsAssignableFrom<IList<CountTeamsResultDto>>(okObjectResult.Value);

            // Count list must contain 4 items.
            Assert.Equal(4, list.Count);

            var item1 = list[0];
            Assert.Equal(4, item1.ItemCount);
            Assert.True(item1.CountOfTeams > 0);

            var item2 = list[1];
            Assert.Equal(3, item2.ItemCount);
            Assert.True(item2.CountOfTeams > 0);

            var item3 = list[2];
            Assert.Equal(2, item3.ItemCount);
            Assert.True(item3.CountOfTeams > 0);

            var item4 = list[3];
            Assert.Equal(1, item4.ItemCount);
            Assert.True(item4.CountOfTeams > 0);
        }
    }
}
