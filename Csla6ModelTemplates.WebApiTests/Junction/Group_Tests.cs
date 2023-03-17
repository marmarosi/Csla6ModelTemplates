using Csla6ModelTemplates.Contracts.Junction.Edit;
using Csla6ModelTemplates.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.WebApiTests.Junction
{
    public class Group_Tests
    {
        #region New

        [Fact]
        public async Task NewGroup_ReturnsNewModel()
        {
            // Arrange
            TestSetup setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<JunctionController>();
            var sut = new JunctionController(logger, setup.PortalFactory, setup.ChildPortalFactory);

            // Act
            ActionResult<GroupDto> actionResult = await sut.GetNewGroup();

            // Assert
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            GroupDto group = okObjectResult.Value as GroupDto;
            Assert.NotNull(group);

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
            // Arrange
            TestSetup setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<JunctionController>();
            var sut = new JunctionController(logger, setup.PortalFactory, setup.ChildPortalFactory);

            // Act
            GroupDto pristineGroup = null;
            GroupPersonDto pristineMember1 = null;
            GroupPersonDto pristineMember2 = null;

            pristineGroup = new GroupDto
            {
                GroupId = null,
                GroupCode = "T-9201",
                GroupName = "Test group number 9201",
                Timestamp = null
            };
            pristineMember1 = new GroupPersonDto
            {
                PersonId = "7adBbqg8nxV",
                PersonName = "Person #11"
            };
            pristineGroup.Persons.Add(pristineMember1);
            pristineMember2 = new GroupPersonDto
            {
                PersonId = "4aoj5R40G1e",
                PersonName = "Person #17"
            };
            pristineGroup.Persons.Add(pristineMember2);

            ActionResult<GroupDto> actionResult = await sut.CreateGroup(pristineGroup);

            // Assert
            CreatedResult createdResult = actionResult.Result as CreatedResult;
            Assert.NotNull(createdResult);

            GroupDto createdGroup = createdResult.Value as GroupDto;
            Assert.NotNull(createdGroup);

            // The group must have new values.
            Assert.NotNull(createdGroup.GroupId);
            Assert.Equal(pristineGroup.GroupCode, createdGroup.GroupCode);
            Assert.Equal(pristineGroup.GroupName, createdGroup.GroupName);
            Assert.NotNull(createdGroup.Timestamp);

            // The persons must have new values.
            Assert.Equal(2, createdGroup.Persons.Count);

            GroupPersonDto createdMember1 = createdGroup.Persons[0];
            Assert.Equal(pristineMember1.PersonId, createdMember1.PersonId);
            Assert.Equal(pristineMember1.PersonName, createdMember1.PersonName);

            GroupPersonDto createdMember2 = createdGroup.Persons[1];
            Assert.Equal(pristineMember2.PersonId, createdMember2.PersonId);
            Assert.Equal(pristineMember2.PersonName, createdMember2.PersonName);
        }

        #endregion

        #region Read

        [Fact]
        public async Task ReadGroup_ReturnsCurrentModel()
        {
            // Arrange
            TestSetup setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<JunctionController>();
            var sut = new JunctionController(logger, setup.PortalFactory, setup.ChildPortalFactory);

            // Act
            ActionResult<GroupDto> actionResult = await sut.GetGroup("6KANyA658o9");

            // Assert
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            GroupDto pristineGroup = okObjectResult.Value as GroupDto;
            Assert.NotNull(pristineGroup);

            // The group code and name must end with 10.
            Assert.Equal("6KANyA658o9", pristineGroup.GroupId);
            Assert.Equal("G-10", pristineGroup.GroupCode);
            Assert.EndsWith("10", pristineGroup.GroupName);
            Assert.NotNull(pristineGroup.Timestamp);

            // The person name must start with Person.
            Assert.True(pristineGroup.Persons.Count > 0);
            foreach (GroupPersonDto groupPerson in pristineGroup.Persons)
            {
                Assert.StartsWith("Person", groupPerson.PersonName);
            }
        }

        #endregion

        #region Update

        [Fact]
        public async Task UpdateGroup_ReturnsUpdatedModel()
        {
            // Arrange
            TestSetup setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<JunctionController>();
            var sutR = new JunctionController(logger, setup.PortalFactory, setup.ChildPortalFactory);
            var sutU = new JunctionController(logger, setup.PortalFactory, setup.ChildPortalFactory);

            // Act
            GroupDto pristineGroup = null;
            GroupPersonDto pristineMember1 = null;
            GroupPersonDto pristineMemberNew = null;

            ActionResult<GroupDto> actionResultR = await sutR.GetGroup("aqL3y3P5dGm");
            OkObjectResult okObjectResultR = actionResultR.Result as OkObjectResult;
            pristineGroup = okObjectResultR.Value as GroupDto;
            pristineMember1 = pristineGroup.Persons[0];

            pristineGroup.GroupCode = "G-1212";
            pristineGroup.GroupName = "Group No. 1212";

            pristineMemberNew = new GroupPersonDto
            {
                PersonId = "a4P18mr5M62",
                PersonName = "New member",
            };
            pristineGroup.Persons.Add(pristineMemberNew);
            ActionResult<GroupDto> actionResultU = await sutU.UpdateGroup(pristineGroup);

            // Assert
            OkObjectResult okObjectResultU = actionResultU.Result as OkObjectResult;
            Assert.NotNull(okObjectResultU);

            GroupDto updatedGroup = okObjectResultU.Value as GroupDto;
            Assert.NotNull(updatedGroup);

            // The group must have new values.
            Assert.Equal(pristineGroup.GroupId, updatedGroup.GroupId);
            Assert.Equal(pristineGroup.GroupCode, updatedGroup.GroupCode);
            Assert.Equal(pristineGroup.GroupName, updatedGroup.GroupName);
            Assert.NotEqual(pristineGroup.Timestamp, updatedGroup.Timestamp);

            Assert.Equal(pristineGroup.Persons.Count, updatedGroup.Persons.Count);

            // Persons must reflect the changes.
            GroupPersonDto updatedMember1 = updatedGroup.Persons[0];
            Assert.Equal(pristineMember1.PersonId, updatedMember1.PersonId);
            Assert.Equal(pristineMember1.PersonName, updatedMember1.PersonName);

            GroupPersonDto createdMemberNew = updatedGroup.Persons[pristineGroup.Persons.Count - 1];
            Assert.Equal(pristineMemberNew.PersonId, createdMemberNew.PersonId);
            Assert.StartsWith("New member", createdMemberNew.PersonName);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task DeleteGroup_ReturnsNothing()
        {
            // Arrange
            TestSetup setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<JunctionController>();
            var sut = new JunctionController(logger, setup.PortalFactory, setup.ChildPortalFactory);

            // Act
            ActionResult actionResult = await sut.DeleteGroup("3Nr8nQenQjA");

            // Assert
            NoContentResult noContentResult = actionResult as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        #endregion
    }
}
