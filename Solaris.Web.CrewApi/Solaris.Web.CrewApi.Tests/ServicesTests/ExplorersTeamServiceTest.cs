using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Extensions.Logging;
using Moq;
using Solaris.Web.CrewApi.Core.Models.Entities;
using Solaris.Web.CrewApi.Core.Models.Helpers.Commons;
using Solaris.Web.CrewApi.Core.Models.Interfaces.Filters;
using Solaris.Web.CrewApi.Core.Repositories.Interfaces;
using Solaris.Web.CrewApi.Infrastructure.Filters;
using Solaris.Web.CrewApi.Infrastructure.Services.Implementations;
using Xunit;

namespace Solaris.Web.CrewApi.Tests.ServicesTests
{
    public class ExplorersTeamServiceTests
    {
        private readonly Mock<IExplorersTeamRepository> m_repositoryMock;
        private readonly ExplorersTeamService m_explorersTeamService;

        public ExplorersTeamServiceTests()
        {
            m_repositoryMock = new Mock<IExplorersTeamRepository>();
            m_explorersTeamService = new ExplorersTeamService(new Mock<ILogger<ExplorersTeamService>>().Object, m_repositoryMock.Object);
        }

        [Fact]
        public async void CreateWithValidData_Ok()
        {
            //Act
            await m_explorersTeamService.CreateExplorersTeamAsync(new ExplorersTeam
            {
                Id = Guid.NewGuid(),
                Name = "A"
            });

            //Assert
            m_repositoryMock.Verify(t => t.CreateAsync(It.IsAny<ExplorersTeam>()), Times.Once);
        }

        [Fact]
        public async void CreateWithInvalidNameAndDistance_Throws()
        {
            //Act

            var invalidName = m_explorersTeamService.CreateExplorersTeamAsync(new ExplorersTeam
            {
                Id = Guid.NewGuid(),
                Name = "a-"
            });

            //Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await invalidName);
            m_repositoryMock.Verify(t => t.CreateAsync(It.IsAny<ExplorersTeam>()), Times.Never);
        }

        [Fact]
        public async void CreateWithRepositoryFail_Throws()
        {
            //Arrange
            m_repositoryMock.Setup(t => t.CreateAsync(It.IsAny<ExplorersTeam>())).ThrowsAsync(new Exception("Mocked"));

            //Act
            var repoFailed = m_explorersTeamService.CreateExplorersTeamAsync(new ExplorersTeam
            {
                Id = Guid.NewGuid(),
                Name = "A"
            });

            //Assert
            await Assert.ThrowsAsync<Exception>(async () => await repoFailed);
        }

        [Fact]
        public async void UpdateWithValidData_Ok()
        {
            //Arrange
            var explorersTeam = new ExplorersTeam
            {
                Id = Guid.NewGuid(),
                Name = "A"
            };
            m_repositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<IFilter<ExplorersTeam>>()))
                .ReturnsAsync(new Tuple<int, List<ExplorersTeam>>(1, new List<ExplorersTeam> {explorersTeam}));

            //Act
            await m_explorersTeamService.UpdateExplorersTeamAsync(explorersTeam);

            //Assert
            m_repositoryMock.Verify(t => t.UpdateAsync(It.IsAny<List<ExplorersTeam>>()), Times.Once);
        }

        [Fact]
        public async void UpdateInvalidExplorersTeam_Throws()
        {
            //Arrange
            m_repositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<IFilter<ExplorersTeam>>()))
                .ReturnsAsync(new Tuple<int, List<ExplorersTeam>>(0, new List<ExplorersTeam>()));

            //Act
            var explorersTeamDoesNotExist = m_explorersTeamService.UpdateExplorersTeamAsync(new ExplorersTeam
            {
                Id = Guid.NewGuid(),
                Name = "A-1"
            });

            //Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await explorersTeamDoesNotExist);
        }

        [Fact]
        public async void DeleteExplorersTeam_Ok()
        {
            //Arrange
            var explorersTeam = new ExplorersTeam
            {
                Id = Guid.NewGuid(),
                Name = "A"
            };
            m_repositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<IFilter<ExplorersTeam>>()))
                .ReturnsAsync(new Tuple<int, List<ExplorersTeam>>(1, new List<ExplorersTeam> {explorersTeam}));

            //Act
            await m_explorersTeamService.DeleteExplorersTeamAsync(explorersTeam.Id);

            //Assert
            m_repositoryMock.Verify(t => t.DeleteAsync(It.IsAny<ExplorersTeam>()), Times.Once);
        }

        [Fact]
        public async void DeleteExplorersTeamForInvalidExplorersTeam_Throws()
        {
            //Arrange
            m_repositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<IFilter<ExplorersTeam>>()))
                .ReturnsAsync(new Tuple<int, List<ExplorersTeam>>(0, new List<ExplorersTeam>()));

            //Act
            var explorersTeamDoesNotExist = m_explorersTeamService.DeleteExplorersTeamAsync(Guid.NewGuid());

            //Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await explorersTeamDoesNotExist);
        }

        [Fact]
        public async void SearchExplorersTeam_Ok()
        {
            //Arrange
            var explorersTeam = new ExplorersTeam
            {
                Id = Guid.NewGuid(),
                Name = "A"
            };
            m_repositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<IFilter<ExplorersTeam>>()))
                .ReturnsAsync(new Tuple<int, List<ExplorersTeam>>(1, new List<ExplorersTeam> {explorersTeam}));

            //Act
            var (count, explorersTeams) = await m_explorersTeamService.SearchExplorersTeamAsync(new Pagination(), new Ordering(), new ExplorersTeamFilter
            {
                SearchTerm = explorersTeam.Id.ToString()
            });

            //Assert
            m_repositoryMock.Verify(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<ExplorersTeamFilter>()), Times.Once);
            Assert.Equal(1, count);
            Assert.Equal(explorersTeam, explorersTeams.First());
        }
    }
}