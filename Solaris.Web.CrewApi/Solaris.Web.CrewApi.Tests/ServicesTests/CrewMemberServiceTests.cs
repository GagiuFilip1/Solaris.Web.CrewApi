using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Moq;
using Solaris.Web.CrewApi.Core.Models.Entities;
using Solaris.Web.CrewApi.Core.Models.Filters.Implementation;
using Solaris.Web.CrewApi.Core.Models.Filters.Interfaces;
using Solaris.Web.CrewApi.Core.Models.Helpers.Commons;
using Solaris.Web.CrewApi.Core.Repositories.Interfaces;
using Solaris.Web.CrewApi.Core.Services.Implementations;
using Xunit;

namespace Solaris.Web.CrewApi.Tests.ServicesTests
{
    public class CrewMemberServiceTests
    {
        private readonly Mock<ICrewMemberRepository> m_repositoryMock;
        private readonly CrewMemberService m_captainService;


        public CrewMemberServiceTests()
        {
            m_repositoryMock = new Mock<ICrewMemberRepository>();
            m_captainService = new CrewMemberService(new Mock<ILogger<CrewMemberService>>().Object, m_repositoryMock.Object);
        }

        [Fact]
        public async void SearchCrewMember_Ok()
        {
            //Arrange
            var captain = new Captain
            {
                Id = Guid.NewGuid(),
                Name = "Test"
            };
            m_repositoryMock.Setup(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<IFilter<CrewMember>>()))
                .ReturnsAsync(new Tuple<int, List<CrewMember>>(1, new List<CrewMember> {captain}));

            //Act
            var (count, captains) = await m_captainService.SearchCrewMemberAsync(new Pagination(), new Ordering(), new CrewMemberFilter
            {
                SearchTerm = captain.Id.ToString()
            });

            //Assert
            m_repositoryMock.Verify(t => t.SearchAsync(It.IsAny<Pagination>(), It.IsAny<Ordering>(), It.IsAny<CrewMemberFilter>()), Times.Once);
            Assert.Equal(1, count);
            Assert.Equal(captain, captains.First());
        }
        
        [Fact]
        public async void CrewMemberCountAsync_Ok()
        {
            //Arrange

            m_repositoryMock.Setup(t => t.CountAsync(It.IsAny<IFilter<CrewMember>>()))
                .ReturnsAsync(1);

            //Act
            var count = await m_captainService.SimpleCountAsync(new CrewMemberFilter());
            //Assert
            m_repositoryMock.Verify(t => t.CountAsync(It.IsAny<CrewMemberFilter>()), Times.Once);
            Assert.Equal(1, count);
        }
    }
}