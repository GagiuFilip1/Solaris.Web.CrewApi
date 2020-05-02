using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Extensions.Logging;
using Moq;
using Solaris.Web.CrewApi.Core.Models.Entities;
using Solaris.Web.CrewApi.Core.Models.Helpers;
using Solaris.Web.CrewApi.Core.Models.Interfaces;
using Solaris.Web.CrewApi.Core.Repositories.Interfaces;
using Solaris.Web.CrewApi.Infrastructure.Filters;
using Solaris.Web.CrewApi.Infrastructure.Services.Implementations;
using Xunit;

namespace Solaris.Web.CrewApi.Tests.ServicesTests
{
    public class RobotServiceTests
    {
        public RobotServiceTests()
        {
            m_repositoryMock = new Mock<IRobotRepository>();
            m_explorersTeamRepositoryMock = new Mock<IExplorersTeamRepository>();
            m_robotService = new RobotService(new Mock<ILogger<RobotService>>().Object, m_repositoryMock.Object, m_explorersTeamRepositoryMock.Object, null, null);
        }

        private readonly Mock<IRobotRepository> m_repositoryMock;
        private readonly Mock<IExplorersTeamRepository> m_explorersTeamRepositoryMock;
        private readonly RobotService m_robotService;

        [Fact]
        public async void CreateWithInvalidData_Throws()
        {
            //Arrange
            var explorersTeam = new ExplorersTeam
            {
                Id = Guid.NewGuid(),
                Name = "Test"
            };
            m_explorersTeamRepositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<ExplorersTeamFilter>()))
                .ReturnsAsync(new Tuple<int, List<ExplorersTeam>>(1, new List<ExplorersTeam> {explorersTeam}));

            //Act
            var invalidData = m_robotService.CreateRobotAsync(new Robot
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                ExplorersTeamId = explorersTeam.Id,
                ProductNumber = "123A"
            });

            //Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await invalidData);
            m_repositoryMock.Verify(t => t.CreateAsync(It.IsAny<Robot>()), Times.Never);
        }

        [Fact]
        public async void CreateWithRepositoryFail_Throws()
        {
            //Arrange
            m_repositoryMock.Setup(t => t.CreateAsync(It.IsAny<Robot>())).ThrowsAsync(new Exception("Mocked"));
            var explorersTeam = new ExplorersTeam
            {
                Id = Guid.NewGuid(),
                Name = "Test"
            };
            m_explorersTeamRepositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<ExplorersTeamFilter>()))
                .ReturnsAsync(new Tuple<int, List<ExplorersTeam>>(1, new List<ExplorersTeam> {explorersTeam}));

            //Act
            var repoFailed = m_robotService.CreateRobotAsync(new Robot
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                ExplorersTeamId = explorersTeam.Id,
                ProductNumber = "123"
            });

            //Assert
            await Assert.ThrowsAsync<Exception>(async () => await repoFailed);
        }

        [Fact]
        public async void CreateWithValidData_Ok()
        {
            //Arrange
            var explorersTeam = new ExplorersTeam
            {
                Id = Guid.NewGuid(),
                Name = "Test"
            };
            m_explorersTeamRepositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<ExplorersTeamFilter>()))
                .ReturnsAsync(new Tuple<int, List<ExplorersTeam>>(1, new List<ExplorersTeam> {explorersTeam}));
            //Act
            await m_robotService.CreateRobotAsync(new Robot
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                ExplorersTeamId = explorersTeam.Id,
                ProductNumber = "123"
            });

            //Assert
            m_repositoryMock.Verify(t => t.CreateAsync(It.IsAny<Robot>()), Times.Once);
        }

        [Fact]
        public async void DeleteRobot_Ok()
        {
            //Arrange
            var robot = new Robot
            {
                Id = Guid.NewGuid(),
                Name = "Test"
            };
            m_repositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<IFilter<Robot>>()))
                .ReturnsAsync(new Tuple<int, List<Robot>>(1, new List<Robot> {robot}));

            //Act
            await m_robotService.DeleteRobotAsync(robot.Id);

            //Assert
            m_repositoryMock.Verify(t => t.DeleteAsync(It.IsAny<Robot>()), Times.Once);
        }

        [Fact]
        public async void DeleteRobotForInvalidRobot_Throws()
        {
            //Arrange
            m_repositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<IFilter<Robot>>()))
                .ReturnsAsync(new Tuple<int, List<Robot>>(0, new List<Robot>()));

            //Act
            var robotDoesNotExist = m_robotService.DeleteRobotAsync(Guid.NewGuid());

            //Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await robotDoesNotExist);
        }

        [Fact]
        public async void SearchRobot_Ok()
        {
            //Arrange
            var robot = new Robot
            {
                Id = Guid.NewGuid(),
                Name = "Test"
            };
            m_repositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<IFilter<Robot>>()))
                .ReturnsAsync(new Tuple<int, List<Robot>>(1, new List<Robot> {robot}));

            //Act
            var (count, robots) = await m_robotService.SearchRobotAsync(new Pagination(), new Ordering(), new RobotFilter
            {
                SearchTerm = robot.Id.ToString()
            });

            //Assert
            m_repositoryMock.Verify(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<RobotFilter>()), Times.Once);
            Assert.Equal(1, count);
            Assert.Equal(robot, robots.First());
        }

        [Fact]
        public async void UpdateInvalidRobot_Throws()
        {
            //Arrange
            m_repositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<IFilter<Robot>>()))
                .ReturnsAsync(new Tuple<int, List<Robot>>(0, new List<Robot>()));
            var explorersTeam = new ExplorersTeam
            {
                Id = Guid.NewGuid(),
                Name = "Test"
            };
            m_explorersTeamRepositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<ExplorersTeamFilter>()))
                .ReturnsAsync(new Tuple<int, List<ExplorersTeam>>(1, new List<ExplorersTeam> {explorersTeam}));

            //Act
            var robotDoesNotExist = m_robotService.UpdateRobotAsync(new Robot
            {
                Id = Guid.NewGuid(),
                Name = "Test"
            });

            //Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await robotDoesNotExist);
        }

        [Fact]
        public async void UpdateWithValidData_Ok()
        {
            //Arrange
            var explorersTeam = new ExplorersTeam
            {
                Id = Guid.NewGuid(),
                Name = "Test"
            };
            var robot = new Robot
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                ExplorersTeamId = explorersTeam.Id,
                ProductNumber = "123"
            };
            m_repositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<IFilter<Robot>>()))
                .ReturnsAsync(new Tuple<int, List<Robot>>(1, new List<Robot> {robot}));
            m_explorersTeamRepositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<ExplorersTeamFilter>()))
                .ReturnsAsync(new Tuple<int, List<ExplorersTeam>>(1, new List<ExplorersTeam> {explorersTeam}));

            //Act
            await m_robotService.UpdateRobotAsync(robot);

            //Assert
            m_repositoryMock.Verify(t => t.UpdateAsync(It.IsAny<Robot>()), Times.Once);
        }
    }
}