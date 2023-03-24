using Csla6ModelTemplates.Contracts.Simple.Set;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Endpoints.Simple;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.EndpointTests.Simple
{
    public class SimpleTeamSet_Tests : TestBase
    {
        #region Read

        [Fact]
        public async Task ReadTeamSet_ReturnsCurrentModels()
        {
            // ********** Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<ReadSet>();
            var sut = new ReadSet(logger, setup.Csla);

            // ********** Act
            var actionResult = await sut.HandleAsync(
                new SimpleTeamSetCriteria { TeamName = "8" }
                );

            // ********** Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult);
            var pristineList = Assert.IsAssignableFrom<IList<SimpleTeamSetItemDto>>(okObjectResult.Value);

            // List must contain 5 items.
            Assert.InRange(pristineList.Count, 1, 10);
        }

        #endregion

        #region Update

        [Fact]
        public async Task UpdateTeamSet_ReturnsUpdatedModels()
        {
            // ********** Arrange
            var setup = TestSetup.GetInstance();
            var loggerR = setup.GetLogger<ReadSet>();
            var loggerU = setup.GetLogger<UpdateSet>();
            var sutR = new ReadSet(loggerR, setup.Csla);
            var sutU = new UpdateSet(loggerU, setup.Csla);

            // ********** Act
            var criteria = new SimpleTeamSetCriteria { TeamName = "8" };
            var actionResultR = await sutR.HandleAsync(criteria);
            var okObjectResultR = Assert.IsType<OkObjectResult>(actionResultR);
            var pristineList = Assert.IsAssignableFrom<IList<SimpleTeamSetItemDto>>(okObjectResultR.Value);

            // Modify an item.
            var pristine = pristineList[0];
            pristine.TeamCode = "T-9101";
            pristine.TeamName = "Test team number 9101";

            // Create new item.
            var pristineNew = new SimpleTeamSetItemDto
            {
                TeamId = null,
                TeamCode = "T-9102",
                TeamName = "Test team number 9102",
                Timestamp = null
            };
            pristineList.Add(pristineNew);

            // Delete an item.
            SimpleTeamSetItemDto pristine3 = pristineList[3];
            var deletedId = pristine3.TeamId;
            pristineList.Remove(pristine3);

            // ********** Act
            var request = new SimpleTeamSetRequest(criteria, (List<SimpleTeamSetItemDto>)pristineList);
            var actionResultU = await sutU.HandleAsync(request);

            // ********** Assert
            if (IsDeadlock(actionResultU, "SimpleTeamSet - Update")) return;
            var okObjectResultU = Assert.IsType<OkObjectResult>(actionResultU);
            var updatedList = Assert.IsAssignableFrom<IList<SimpleTeamSetItemDto>>(okObjectResultU.Value);

            // The updated team must have new values.
            var updated = updatedList[0];

            Assert.Equal(pristine.TeamId, updated.TeamId);
            Assert.Equal(pristine.TeamCode, updated.TeamCode);
            Assert.Equal(pristine.TeamName, updated.TeamName);
            Assert.NotEqual(pristine.Timestamp, updated.Timestamp);

            // The created team must have new values.
            var created = updatedList
                .FirstOrDefault(o => o.TeamCode == pristineNew.TeamCode);
            Assert.NotNull(created);

            Assert.NotNull(created.TeamId);
            Assert.Equal(pristineNew.TeamCode, created.TeamCode);
            Assert.Equal(pristineNew.TeamName, created.TeamName);
            Assert.NotNull(created.Timestamp);

            // The deleted team must have gone.
            var deleted = updatedList
                .FirstOrDefault(o => o.TeamId == deletedId);
            Assert.Null(deleted);
        }

        #endregion
    }
}
