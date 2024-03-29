﻿using Csla6ModelTemplates.Contracts.Simple.List;
using Csla6ModelTemplates.Endpoints.Simple;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;

namespace Csla6ModelTemplates.EndpointTests.Simple
{
    public class SimpleTeamList_Tests
    {
        [Fact]
        public async Task GetTeamList_ReturnsAList()
        {
            // ********** Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<List>();
            var sut = new List(logger, setup.Csla);

            // ********** Act
            var actionResult = await sut.HandleAsync(
                new SimpleTeamListCriteria { TeamName = "9" }
                );

            // ********** Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult);
            var list = Assert.IsAssignableFrom<IList<SimpleTeamListItemDto>>(okObjectResult.Value);

            // The list must have 5 items.
            Assert.InRange(list.Count, 1, 10);

            // The code and names must end with 9.
            foreach (var item in list)
            {
                Assert.EndsWith("9", item.TeamCode);
                Assert.EndsWith("9", item.TeamName);
            }
        }
    }
}
