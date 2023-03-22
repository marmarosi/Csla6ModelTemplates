using Csla6ModelTemplates.Contracts.Simple.Set;
using Csla6ModelTemplates.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.WebApiTests.Simple
{
    public class SimpleTeamSet_Tests
    {
        #region Read

        [Fact]
        public async Task ReadTeamSet_ReturnsCurrentModels()
        {
            // Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<SimpleController>();
            var sut = new SimpleController(logger, setup.Csla);

            // Act
            SimpleTeamSetCriteria criteria = new SimpleTeamSetCriteria { TeamName = "8" };
            ActionResult<List<SimpleTeamSetItemDto>> actionResult = await sut.GetTeamSet(criteria);

            // Assert
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var pristineList = okObjectResult.Value as List<SimpleTeamSetItemDto>;
            Assert.NotNull(pristineList);

            // List must contain 5 items.
            Assert.True(pristineList.Count > 3);
        }

        #endregion

        #region Update

        [Fact]
        public async Task UpdateTeamSet_ReturnsUpdatedModels()
        {
            // Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<SimpleController>();
            var sutR = new SimpleController(logger, setup.Csla);
            var sutU = new SimpleController(logger, setup.Csla);

            // Act
            var criteria = new SimpleTeamSetCriteria { TeamName = "8" };
            ActionResult<List<SimpleTeamSetItemDto>> actionResultR = await sutR.GetTeamSet(criteria);
            var okObjectResultR = actionResultR.Result as OkObjectResult;
            var pristineList = okObjectResultR.Value as List<SimpleTeamSetItemDto>;

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

            var actionResultU = await sutU.UpdateTeamSet(
                criteria,
                pristineList
                );

            // Assert
            var okObjectResultU = actionResultU.Result as OkObjectResult;
            Assert.NotNull(okObjectResultU);

            var updatedList = okObjectResultU.Value as List<SimpleTeamSetItemDto>;
            Assert.NotNull(updatedList);

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
