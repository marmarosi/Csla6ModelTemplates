using Csla6ModelTemplates.Contracts.Arrangement.Sorting;
using Csla6ModelTemplates.Dal.Contracts;
using Csla6ModelTemplates.Endpoints.Arrangement;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.EndpointTests.Arrangement
{
    public class SortedTeamList_Tests
    {
        [Fact]
        public async Task GetSortedTeamList_ReturnsAList()
        {
            // ********** Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<Sorting>();
            var sut = new Sorting(logger, setup.Csla);

            // ********** Act
            var actionResult = await sut.HandleAsync(
                new SortedTeamListCriteria
                {
                    TeamName = "5",
                    SortBy = SortedTeamListSortBy.TeamCode,
                    SortDirection = SortDirection.Descending
                });

            // ********** Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult);
            var list = Assert.IsAssignableFrom<IList<SortedTeamListItemDto>>(okObjectResult.Value);

            // The list must have 6 items.
            Assert.Equal(6, list.Count);

            // The code and names must end with 5 or 50.
            foreach (var item in list)
            {
                Assert.True(item.TeamCode.EndsWith("5") || item.TeamCode.EndsWith("50"));
                Assert.True(item.TeamName.EndsWith("5") || item.TeamName.EndsWith("50"));
            }
        }
    }
}
