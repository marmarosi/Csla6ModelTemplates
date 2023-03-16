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
            TestSetup setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<SelectionController>();
            var sut = new SelectionController(logger, setup.PortalFactory, setup.ChildPortalFactory);

            // Act
            TeamKeyChoiceCriteria criteria = new TeamKeyChoiceCriteria { TeamName = "7" };
            ActionResult<List<KeyNameOptionDto>> actionResult = await sut.GetTeamChoiceWithKey(criteria);

            // Assert
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            IList<KeyNameOptionDto> choice = okObjectResult.Value as IList<KeyNameOptionDto>;
            Assert.NotNull(choice);

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
