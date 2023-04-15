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
            // ********** Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<SelectionController>();
            var sut = new SelectionController(logger, setup.Csla);

            // ********** Act
            var actionResult = await sut.GetTeamChoiceWithCode(
                new TeamCodeChoiceCriteria { TeamName = "9" }
                );

            // ********** Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult);
            var choice = Assert.IsAssignableFrom<IList<CodeNameOptionDto>>(okObjectResult.Value);

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
