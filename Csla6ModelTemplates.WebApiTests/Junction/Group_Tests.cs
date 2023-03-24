using Csla6ModelTemplates.Contracts.Junction.Edit;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.WebApiTests.Junction
{
    public class Group_Tests : TestBase
    {
        #region New

        [Fact]
        public async Task NewGroup_ReturnsNewModel()
        {
            // ********** Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<JunctionController>();
            var sut = new JunctionController(logger, setup.Csla);

            // ********** Act
            var actionResult = await sut.GetNewGroup();

            // ********** Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult);
            var group = Assert.IsAssignableFrom<GroupDto>(okObjectResult.Value);

            // The code and name must miss.
            Assert.Empty(group.GroupCode);
            Assert.Empty(group.GroupName);
            Assert.Null(group.Timestamp);
            Assert.Empty(group.Persons);
        }

        #endregion

        #region Create

        [Fact]
        public async Task CreateGroup_ReturnsCreatedModel()
        {
            // ********** Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<JunctionController>();
            var sut = new JunctionController(logger, setup.Csla);

            // ********** Act
            var pristineGroup = new GroupDto
            {
                GroupId = null,
                GroupCode = "T-9201",
                GroupName = "Test group number 9201",
                Timestamp = null
            };
            var pristineMember1 = new GroupPersonDto("7adBbqg8nxV", "Person #11");
            pristineGroup.Persons.Add(pristineMember1);
            var pristineMember2 = new GroupPersonDto("4aoj5R40G1e", "Person #17");
            pristineGroup.Persons.Add(pristineMember2);

            var actionResult = await sut.CreateGroup(pristineGroup);

            // ********** Assert
            if (IsDeadlock(actionResult, "Group - Create")) return;
            var createdResult = Assert.IsType<CreatedResult>(actionResult);
            var createdGroup = Assert.IsAssignableFrom<GroupDto>(createdResult.Value);

            // The group must have new values.
            Assert.NotNull(createdGroup.GroupId);
            Assert.Equal(pristineGroup.GroupCode, createdGroup.GroupCode);
            Assert.Equal(pristineGroup.GroupName, createdGroup.GroupName);
            Assert.NotNull(createdGroup.Timestamp);

            // The persons must have new values.
            Assert.Equal(2, createdGroup.Persons.Count);

            var createdMember1 = createdGroup.Persons[0];
            Assert.Equal(pristineMember1.PersonId, createdMember1.PersonId);
            Assert.Equal(pristineMember1.PersonName, createdMember1.PersonName);

            var createdMember2 = createdGroup.Persons[1];
            Assert.Equal(pristineMember2.PersonId, createdMember2.PersonId);
            Assert.Equal(pristineMember2.PersonName, createdMember2.PersonName);
        }

        #endregion

        #region Read

        [Fact]
        public async Task ReadGroup_ReturnsCurrentModel()
        {
            // ********** Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<JunctionController>();
            var sut = new JunctionController(logger, setup.Csla);

            // ********** Act
            var actionResult = await sut.GetGroup("6KANyA658o9");

            // ********** Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult);
            var group = Assert.IsAssignableFrom<GroupDto>(okObjectResult.Value);

            // The group code and name must end with 10.
            Assert.Equal("6KANyA658o9", group.GroupId);
            Assert.Equal("G-10", group.GroupCode);
            Assert.EndsWith("10", group.GroupName);
            Assert.NotNull(group.Timestamp);

            // The person name must start with Person.
            Assert.True(group.Persons.Count > 0);
            foreach (var person in group.Persons)
            {
                Assert.StartsWith("Person", person.PersonName);
            }
        }

        #endregion

        #region Update

        [Fact]
        public async Task UpdateGroup_ReturnsUpdatedModel()
        {
            // ********** Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<JunctionController>();
            var sutR = new JunctionController(logger, setup.Csla);
            var sutU = new JunctionController(logger, setup.Csla);

            // ********** Act
            var actionResultR = await sutR.GetGroup("aqL3y3P5dGm");
            var okObjectResultR = Assert.IsType<OkObjectResult>(actionResultR);
            var pristineGroup = Assert.IsAssignableFrom<GroupDto>(okObjectResultR.Value);
            var pristineMember1 = pristineGroup.Persons[0];

            pristineGroup.GroupCode = "G-1212";
            pristineGroup.GroupName = "Group No. 1212";

            var pristineMemberNew = new GroupPersonDto("a4P18mr5M62", "New member");
            pristineGroup.Persons.Add(pristineMemberNew);

            var actionResultU = await sutU.UpdateGroup(pristineGroup);

            // ********** Assert
            if (IsDeadlock(actionResultU, "Group - Update")) return;
            var okObjectResultU = Assert.IsType<OkObjectResult>(actionResultU);
            var updatedGroup = Assert.IsAssignableFrom<GroupDto>(okObjectResultU.Value);

            // The group must have new values.
            Assert.Equal(pristineGroup.GroupId, updatedGroup.GroupId);
            Assert.Equal(pristineGroup.GroupCode, updatedGroup.GroupCode);
            Assert.Equal(pristineGroup.GroupName, updatedGroup.GroupName);
            Assert.NotEqual(pristineGroup.Timestamp, updatedGroup.Timestamp);

            Assert.Equal(pristineGroup.Persons.Count, updatedGroup.Persons.Count);

            // Persons must reflect the changes.
            var updatedMember1 = updatedGroup.Persons[0];
            Assert.Equal(pristineMember1.PersonId, updatedMember1.PersonId);
            Assert.Equal(pristineMember1.PersonName, updatedMember1.PersonName);

            var createdMemberNew = updatedGroup.Persons[pristineGroup.Persons.Count - 1];
            Assert.Equal(pristineMemberNew.PersonId, createdMemberNew.PersonId);
            Assert.StartsWith("New member", createdMemberNew.PersonName);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task DeleteGroup_ReturnsNothing()
        {
            // ********** Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<JunctionController>();
            var sut = new JunctionController(logger, setup.Csla);

            // ********** Act
            var actionResult = await sut.DeleteGroup("3Nr8nQenQjA");

            // ********** Assert
            if (IsDeadlock(actionResult, "Group - Delete")) return;
            var noContentResult = Assert.IsType<NoContentResult>(actionResult);

            Assert.Equal(204, noContentResult.StatusCode);
        }

        #endregion
    }
}
