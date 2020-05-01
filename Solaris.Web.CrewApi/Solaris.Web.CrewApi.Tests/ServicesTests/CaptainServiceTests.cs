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
    public class CaptainServiceTests
    {
        public CaptainServiceTests()
        {
            m_repositoryMock = new Mock<ICaptainRepository>();
            m_explorersTeamRepositoryMock = new Mock<IExplorersTeamRepository>();
            m_captainService = new CaptainService(new Mock<ILogger<CaptainService>>().Object, m_repositoryMock.Object, m_explorersTeamRepositoryMock.Object);
        }

        private readonly Mock<ICaptainRepository> m_repositoryMock;
        private readonly Mock<IExplorersTeamRepository> m_explorersTeamRepositoryMock;
        private readonly CaptainService m_captainService;

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
            var invalidData = m_captainService.CreateCaptainAsync(new Captain
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                ExplorersTeamId = explorersTeam.Id,
                Email = "@invalid"
            });

            //Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await invalidData);
            m_repositoryMock.Verify(t => t.CreateAsync(It.IsAny<Captain>()), Times.Never);
        }

        [Fact]
        public async void CreateWithRepositoryFail_Throws()
        {
            //Arrange
            m_repositoryMock.Setup(t => t.CreateAsync(It.IsAny<Captain>())).ThrowsAsync(new Exception("Mocked"));
            var explorersTeam = new ExplorersTeam
            {
                Id = Guid.NewGuid(),
                Name = "Test"
            };
            m_explorersTeamRepositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<ExplorersTeamFilter>()))
                .ReturnsAsync(new Tuple<int, List<ExplorersTeam>>(1, new List<ExplorersTeam> {explorersTeam}));

            //Act
            var repoFailed = m_captainService.CreateCaptainAsync(new Captain
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                ExplorersTeamId = explorersTeam.Id,
                Age = 40,
                Email = "ok@ok.com"
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
            await m_captainService.CreateCaptainAsync(new Captain
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                ExplorersTeamId = explorersTeam.Id,
                Email = "captain@email.com",
                Age = 40
            });

            //Assert
            m_repositoryMock.Verify(t => t.CreateAsync(It.IsAny<Captain>()), Times.Once);
        }

        [Fact]
        public async void DeleteCaptain_Ok()
        {
            //Arrange
            var captain = new Captain
            {
                Id = Guid.NewGuid(),
                Name = "Test"
            };
            m_repositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<IFilter<Captain>>()))
                .ReturnsAsync(new Tuple<int, List<Captain>>(1, new List<Captain> {captain}));

            //Act
            await m_captainService.DeleteCaptainAsync(captain.Id);

            //Assert
            m_repositoryMock.Verify(t => t.DeleteAsync(It.IsAny<Captain>()), Times.Once);
        }

        [Fact]
        public async void DeleteCaptainForInvalidCaptain_Throws()
        {
            //Arrange
            m_repositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<IFilter<Captain>>()))
                .ReturnsAsync(new Tuple<int, List<Captain>>(0, new List<Captain>()));

            //Act
            var captainDoesNotExist = m_captainService.DeleteCaptainAsync(Guid.NewGuid());

            //Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await captainDoesNotExist);
        }

        [Fact]
        public async void SearchCaptain_Ok()
        {
            //Arrange
            var captain = new Captain
            {
                Id = Guid.NewGuid(),
                Name = "Test"
            };
            m_repositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<IFilter<Captain>>()))
                .ReturnsAsync(new Tuple<int, List<Captain>>(1, new List<Captain> {captain}));

            //Act
            var (count, captains) = await m_captainService.SearchCaptainAsync(new Pagination(), new Ordering(), new CaptainFilter
            {
                SearchTerm = captain.Id.ToString()
            });

            //Assert
            m_repositoryMock.Verify(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<CaptainFilter>()), Times.Once);
            Assert.Equal(1, count);
            Assert.Equal(captain, captains.First());
        }

        [Fact]
        public async void UpdateInvalidCaptain_Throws()
        {
            //Arrange
            m_repositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<IFilter<Captain>>()))
                .ReturnsAsync(new Tuple<int, List<Captain>>(0, new List<Captain>()));
            var explorersTeam = new ExplorersTeam
            {
                Id = Guid.NewGuid(),
                Name = "Test"
            };
            m_explorersTeamRepositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<ExplorersTeamFilter>()))
                .ReturnsAsync(new Tuple<int, List<ExplorersTeam>>(1, new List<ExplorersTeam> {explorersTeam}));

            //Act
            var captainDoesNotExist = m_captainService.UpdateCaptainAsync(new Captain
            {
                Id = Guid.NewGuid(),
                Name = "Test"
            });

            //Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await captainDoesNotExist);
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
            var captain = new Captain
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                ExplorersTeamId = explorersTeam.Id,
                Age = 40,
                Email = "ok@ok.com"
            };
            m_repositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<IFilter<Captain>>()))
                .ReturnsAsync(new Tuple<int, List<Captain>>(1, new List<Captain> {captain}));
            m_explorersTeamRepositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<ExplorersTeamFilter>()))
                .ReturnsAsync(new Tuple<int, List<ExplorersTeam>>(1, new List<ExplorersTeam> {explorersTeam}));

            //Act
            await m_captainService.UpdateCaptainAsync(captain);

            //Assert
            m_repositoryMock.Verify(t => t.UpdateAsync(It.IsAny<Captain>()), Times.Once);
        }
    }
}