using Csla6ModelTemplates.Contracts.Selection.WithKey;
using Csla6ModelTemplates.Dal.Contracts;
using Csla6ModelTemplates.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.WebApiTests.Selection
{
    public class TeamKeyChoice_Tests
    {
        [Fact]
        public async Task GetTeamChoiceWithKey_ReturnsAChoice()
        {
            // Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<SelectionController>();
            var sut = new SelectionController(logger, setup.Csla);

            // Act
            var actionResult = await sut.GetTeamChoiceWithKey(
                new TeamKeyChoiceCriteria { TeamName = "7" }
                );

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult);
            var choice = Assert.IsAssignableFrom<IList<KeyNameOptionDto>>(okObjectResult.Value);

            // The choice must have 5 items.
            Assert.Equal(5, choice.Count);

            // The names must end with 7.
            foreach (var item in choice)
            {
                Assert.EndsWith("7", item.Name);
            }
        }
    }
}
