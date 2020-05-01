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
    public class ShuttleServiceTests
    {
        public ShuttleServiceTests()
        {
            m_repositoryMock = new Mock<IShuttleRepository>();
            m_explorersTeamRepositoryMock = new Mock<IExplorersTeamRepository>();
            m_shuttleService = new ShuttleService(new Mock<ILogger<ShuttleService>>().Object, m_repositoryMock.Object, m_explorersTeamRepositoryMock.Object);
        }

        private readonly Mock<IShuttleRepository> m_repositoryMock;
        private readonly Mock<IExplorersTeamRepository> m_explorersTeamRepositoryMock;
        private readonly ShuttleService m_shuttleService;

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
            var invalidData = m_shuttleService.CreateShuttleAsync(new Shuttle
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                ExplorersTeamId = explorersTeam.Id,
                ShipNumber = "A!"
            });

            //Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await invalidData);
            m_repositoryMock.Verify(t => t.CreateAsync(It.IsAny<Shuttle>()), Times.Never);
        }

        [Fact]
        public async void CreateWithRepositoryFail_Throws()
        {
            //Arrange
            m_repositoryMock.Setup(t => t.CreateAsync(It.IsAny<Shuttle>())).ThrowsAsync(new Exception("Mocked"));
            var explorersTeam = new ExplorersTeam
            {
                Id = Guid.NewGuid(),
                Name = "Test"
            };
            m_explorersTeamRepositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<ExplorersTeamFilter>()))
                .ReturnsAsync(new Tuple<int, List<ExplorersTeam>>(1, new List<ExplorersTeam> {explorersTeam}));

            //Act
            var repoFailed = m_shuttleService.CreateShuttleAsync(new Shuttle
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                ExplorersTeamId = explorersTeam.Id,
                ShipNumber = "A1"
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
            await m_shuttleService.CreateShuttleAsync(new Shuttle
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                ExplorersTeamId = explorersTeam.Id,
                ShipNumber = "A1"
            });

            //Assert
            m_repositoryMock.Verify(t => t.CreateAsync(It.IsAny<Shuttle>()), Times.Once);
        }

        [Fact]
        public async void DeleteShuttle_Ok()
        {
            //Arrange
            var shuttle = new Shuttle
            {
                Id = Guid.NewGuid(),
                Name = "Test"
            };
            m_repositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<IFilter<Shuttle>>()))
                .ReturnsAsync(new Tuple<int, List<Shuttle>>(1, new List<Shuttle> {shuttle}));

            //Act
            await m_shuttleService.DeleteShuttleAsync(shuttle.Id);

            //Assert
            m_repositoryMock.Verify(t => t.DeleteAsync(It.IsAny<Shuttle>()), Times.Once);
        }

        [Fact]
        public async void DeleteShuttleForInvalidShuttle_Throws()
        {
            //Arrange
            m_repositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<IFilter<Shuttle>>()))
                .ReturnsAsync(new Tuple<int, List<Shuttle>>(0, new List<Shuttle>()));

            //Act
            var shuttleDoesNotExist = m_shuttleService.DeleteShuttleAsync(Guid.NewGuid());

            //Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await shuttleDoesNotExist);
        }

        [Fact]
        public async void SearchShuttle_Ok()
        {
            //Arrange
            var shuttle = new Shuttle
            {
                Id = Guid.NewGuid(),
                Name = "Test"
            };
            m_repositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<IFilter<Shuttle>>()))
                .ReturnsAsync(new Tuple<int, List<Shuttle>>(1, new List<Shuttle> {shuttle}));

            //Act
            var (count, shuttles) = await m_shuttleService.SearchShuttleAsync(new Pagination(), new Ordering(), new ShuttleFilter
            {
                SearchTerm = shuttle.Id.ToString()
            });

            //Assert
            m_repositoryMock.Verify(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<ShuttleFilter>()), Times.Once);
            Assert.Equal(1, count);
            Assert.Equal(shuttle, shuttles.First());
        }

        [Fact]
        public async void UpdateInvalidShuttle_Throws()
        {
            //Arrange
            m_repositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<IFilter<Shuttle>>()))
                .ReturnsAsync(new Tuple<int, List<Shuttle>>(0, new List<Shuttle>()));
            var explorersTeam = new ExplorersTeam
            {
                Id = Guid.NewGuid(),
                Name = "Test"
            };
            m_explorersTeamRepositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<ExplorersTeamFilter>()))
                .ReturnsAsync(new Tuple<int, List<ExplorersTeam>>(1, new List<ExplorersTeam> {explorersTeam}));

            //Act
            var shuttleDoesNotExist = m_shuttleService.UpdateShuttleAsync(new Shuttle
            {
                Id = Guid.NewGuid(),
                Name = "Test"
            });

            //Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await shuttleDoesNotExist);
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
            var shuttle = new Shuttle
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                ExplorersTeamId = explorersTeam.Id,
                ShipNumber = "A1"
            };
            m_repositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<IFilter<Shuttle>>()))
                .ReturnsAsync(new Tuple<int, List<Shuttle>>(1, new List<Shuttle> {shuttle}));
            m_explorersTeamRepositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<ExplorersTeamFilter>()))
                .ReturnsAsync(new Tuple<int, List<ExplorersTeam>>(1, new List<ExplorersTeam> {explorersTeam}));

            //Act
            await m_shuttleService.UpdateShuttleAsync(shuttle);

            //Assert
            m_repositoryMock.Verify(t => t.UpdateAsync(It.IsAny<Shuttle>()), Times.Once);
        }
    }
}