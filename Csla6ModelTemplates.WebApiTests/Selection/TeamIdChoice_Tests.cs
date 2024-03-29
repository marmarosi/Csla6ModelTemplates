﻿using Csla6ModelTemplates.Contracts.Selection.WithId;
using Csla6ModelTemplates.Dal.Contracts;
using Csla6ModelTemplates.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.WebApiTests.Selection
{
    public class TeamIdChoice_Tests
    {
        [Fact]
        public async Task GetTeamChoiceWithId_ReturnsAChoice()
        {
            // ********** Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<SelectionController>();
            var sut = new SelectionController(logger, setup.Csla);

            // ********** Act
            var actionResult = await sut.GetTeamChoiceWithId(
                new TeamIdChoiceCriteria { TeamName = "0" }
                );

            // ********** Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult);
            var choice = Assert.IsAssignableFrom<IList<IdNameOptionDto>>(okObjectResult.Value);

            // The choice must have 5 items.
            Assert.Equal(5, choice.Count);

            // The names must end with 0.
            foreach (var item in choice)
            {
                Assert.EndsWith("0", item.Name);
            }
        }
    }
}
