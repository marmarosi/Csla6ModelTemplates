using Csla6ModelTemplates.Contracts.Selection.WithCode;
using Csla6ModelTemplates.Dal.Contracts;
using Csla6ModelTemplates.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.WebApiTests.Selection
{
    public class TeamCodeChoice_Tests
    {
        [Fact]
        public async Task GetTeamChoiceWithCode_ReturnsAChoice()
        {
            // Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<SelectionController>();
            var sut = new SelectionController(logger, setup.Csla);

            // Act
            ActionResult<List<CodeNameOptionDto>> actionResult = await sut.GetTeamChoiceWithCode(
                new TeamCodeChoiceCriteria { TeamName = "9" }
                );

            // Assert
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var choice = okObjectResult.Value as IList<CodeNameOptionDto>;
            Assert.NotNull(choice);

            // The choice must have 5 items.
            Assert.Equal(5, choice.Count);

            // The codes and names must end with 9.
            foreach (var option in choice)
            {
                Assert.EndsWith("9", option.Code);
                Assert.EndsWith("9", option.Name);
            }
        }
    }
}
