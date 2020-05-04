using System;
using System.Collections;
using System.Linq;
using Solaris.Web.CrewApi.Core.Enums;
using Solaris.Web.CrewApi.Core.Models.Entities;
using Solaris.Web.CrewApi.Core.Models.Filters.Implementation;
using Solaris.Web.CrewApi.Core.Models.Helpers.Commons;
using Solaris.Web.CrewApi.Infrastructure.Repositories.Implementations;
using Solaris.Web.CrewApi.Tests.Utils;
using Xunit;
namespace Solaris.Web.CrewApi.Tests.RepositoryTests
{
    public class CrewMemberRepositoryTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture m_databaseFixture;
        private readonly CrewMemberRepository m_repository;

        public CrewMemberRepositoryTests(DatabaseFixture databaseFixture)
        {
            m_databaseFixture = databaseFixture;
            m_repository = new CrewMemberRepository(m_databaseFixture.DataContext);
        }

        [Fact]
        public async void SimpleSearch_Ok()
        {
            //ACT
            var (count, crewMembers) = await m_repository.SearchAsync(new Pagination(), new Ordering(), new CrewMemberFilter());
            //ASSERT
            Assert.Equal(5, count);
            Assert.Equal(DatabaseSeed.Captain1Id, crewMembers.First(t => t.Id.Equals(DatabaseSeed.Captain1Id)).Id);
            Assert.Equal(DatabaseSeed.Captain2Id, crewMembers.First(t => t.Id.Equals(DatabaseSeed.Captain2Id)).Id);
            Assert.Equal(DatabaseSeed.RobotFirstTeam1Id, crewMembers.First(t => t.Id.Equals(DatabaseSeed.RobotFirstTeam1Id)).Id);
            Assert.Equal(DatabaseSeed.RobotFirstTeam2Id, crewMembers.First(t => t.Id.Equals(DatabaseSeed.RobotFirstTeam2Id)).Id);
            Assert.Equal(DatabaseSeed.RobotSecondTeam1Id, crewMembers.First(t => t.Id.Equals(DatabaseSeed.RobotSecondTeam1Id)).Id);
        }

        [Fact]
        public async void SearchWithPagination_Ok()
        {
            //ACT
            var (countNoOffset, crewMembersNoOffset) = await m_repository.SearchAsync(new Pagination
            {
                Take = 1,
                Offset = 0
            }, new Ordering(), new CrewMemberFilter());

            var (countWithOffset, crewMembersOffset) = await m_repository.SearchAsync(new Pagination
            {
                Take = 1,
                Offset = 1
            }, new Ordering(), new CrewMemberFilter());

            //ASSERT
            Assert.Equal(5, countNoOffset);
            Assert.Equal(5, countWithOffset);
            Assert.Single((IEnumerable) crewMembersOffset);
            Assert.Single((IEnumerable) crewMembersNoOffset);
        }

        [Fact]
        public async void SearchWithOrdering_Ok()
        {
            //ACT
            var (_, descResult) = await m_repository.SearchAsync(
                new Pagination(),
                new Ordering
                {
                    OrderBy = nameof(CrewMember.Name),
                    OrderDirection = OrderDirection.Desc
                },
                new CrewMemberFilter());

            var (_, ascResult) = await m_repository.SearchAsync(
                new Pagination(),
                new Ordering
                {
                    OrderBy = nameof(CrewMember.Name),
                    OrderDirection = OrderDirection.Asc
                },
                new CrewMemberFilter());
            //ASSERT

            Assert.Equal(descResult.Count, ascResult.Count);
            Assert.Equal(descResult.First().Id, ascResult.Last().Id);
            Assert.Equal(descResult.First().Name, m_databaseFixture.DataContext.CrewMembers.First(t => t.Id.Equals(DatabaseSeed.RobotSecondTeam1Id)).Name);
            Assert.Equal(ascResult.First().Name, m_databaseFixture.DataContext.CrewMembers.First(t => t.Id.Equals(DatabaseSeed.Captain2Id)).Name);
        }

        [Fact]
        public async void SearchWithFiltering_Ok()
        {
            //Arrange
            var firstCrewMember = m_databaseFixture.DataContext.CrewMembers.First(t => t.Id.Equals(DatabaseSeed.Captain1Id));
            //ACT
            var (_, idFiltered) = await m_repository.SearchAsync(
                new Pagination(),
                new Ordering(),
                new CrewMemberFilter
                {
                    SearchTerm = firstCrewMember.Id.ToString()
                });

            var (_, nameFiltered) = await m_repository.SearchAsync(
                new Pagination(),
                new Ordering(),
                new CrewMemberFilter
                {
                    SearchTerm = firstCrewMember.Name
                });
            
            //ASSERT
            Assert.Equal(firstCrewMember.Id, idFiltered.First().Id);
            Assert.Equal(firstCrewMember.Name, nameFiltered.First().Name);
            Assert.Single(nameFiltered);
        }

        [Fact]
        public async void CountWithoutFiltering_Ok()
        {
            //Act
            var count = await m_repository.CountAsync(new CrewMemberFilter());
            
            //Assert
            Assert.Equal(5, count);
        }
        
        [Fact]
        public async void CountWithFiltering_Ok()
        {
            //Act
            var count = await m_repository.CountAsync(new CrewMemberFilter
            {
                SearchTerm = nameof(CrewMemberType.Robot)
            });
            
            //Assert
            Assert.Equal(3, count);
        }
    }
}