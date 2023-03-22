using Csla6ModelTemplates.Contracts.Simple.List;
using Csla6ModelTemplates.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.WebApiTests.Simple
{
    public class SimpleTeamList_Tests
    {
        [Fact]
        public async Task GetTeamList_ReturnsAList()
        {
            // Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<SimpleController>();
            var sut = new SimpleController(logger, setup.Csla);

            // Act
            ActionResult<List<SimpleTeamListItemDto>> actionResult = await sut.GetTeamList(
                new SimpleTeamListCriteria { TeamName = "9" }
                );

            // Assert
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var list = okObjectResult.Value as List<SimpleTeamListItemDto>;
            Assert.NotNull(list);

            // The list must have 5 items.
            Assert.Equal(5, list.Count);

            // The code and names must end with 9.
            foreach (var item in list)
            {
                Assert.EndsWith("9", item.TeamCode);
                Assert.EndsWith("9", item.TeamName);
            }
        }
    }
}
