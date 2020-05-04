using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Solaris.Web.CrewApi.Core.Models.Entities;
using Solaris.Web.CrewApi.Core.Models.Filters.Implementation;
using Solaris.Web.CrewApi.Core.Models.Helpers.Commons;
using Solaris.Web.CrewApi.Core.Repositories.Interfaces;
using Solaris.Web.CrewApi.Infrastructure.Repositories.Implementations;
using Solaris.Web.CrewApi.Tests.Utils;
using Xunit;

namespace Solaris.Web.CrewApi.Tests.RepositoryTests
{
    public class ExplorersTeamRepositoryTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture m_databaseFixture;
        private readonly IExplorersTeamRepository m_repository;

        public ExplorersTeamRepositoryTests(DatabaseFixture databaseFixture)
        {
            m_databaseFixture = databaseFixture;
            m_repository = new ExplorersTeamRepository(m_databaseFixture.DataContext);
        }

        [Fact]
        public async void SimpleSearch_Ok()
        {
            //ACT
            var (count, explorersTeams) = await m_repository.SearchAsync(new Pagination(), new Ordering(), new ExplorersTeamFilter());
            //ASSERT
            Assert.Equal(2, count);
            Assert.Equal(DatabaseSeed.ExplorersTeam1Id, explorersTeams.First(t => t.Id.Equals(DatabaseSeed.ExplorersTeam1Id)).Id);
            Assert.Equal(DatabaseSeed.ExplorersTeam2Id, explorersTeams.First(t => t.Id.Equals(DatabaseSeed.ExplorersTeam2Id)).Id);
        }

        [Fact]
        public async void SearchWithPagination_Ok()
        {
            //ACT
            var (countNoOffset, explorersTeamsNoOffset) = await m_repository.SearchAsync(new Pagination
            {
                Take = 1,
                Offset = 0
            }, new Ordering(), new ExplorersTeamFilter());

            var (countWithOffset, explorersTeamsOffset) = await m_repository.SearchAsync(new Pagination
            {
                Take = 1,
                Offset = 1
            }, new Ordering(), new ExplorersTeamFilter());

            //ASSERT
            Assert.Equal(2, countNoOffset);
            Assert.Equal(2, countWithOffset);
            Assert.Single((IEnumerable) explorersTeamsOffset);
            Assert.Single((IEnumerable) explorersTeamsNoOffset);
        }

        [Fact]
        public async void SearchWithOrdering_Ok()
        {
            //ACT
            var (_, descResult) = await m_repository.SearchAsync(
                new Pagination(),
                new Ordering
                {
                    OrderBy = nameof(ExplorersTeam.Name),
                    OrderDirection = OrderDirection.Desc
                },
                new ExplorersTeamFilter());

            var (_, ascResult) = await m_repository.SearchAsync(
                new Pagination(),
                new Ordering
                {
                    OrderBy = nameof(ExplorersTeam.Name),
                    OrderDirection = OrderDirection.Asc
                },
                new ExplorersTeamFilter());
            //ASSERT

            Assert.Equal(descResult.Count, ascResult.Count);
            Assert.Equal(descResult.First().Id, ascResult.Last().Id);
            Assert.Equal(descResult.First().Name, m_databaseFixture.DataContext.ExplorersTeams.First(t => t.Id.Equals(DatabaseSeed.ExplorersTeam2Id)).Name);
            Assert.Equal(ascResult.First().Name, m_databaseFixture.DataContext.ExplorersTeams.First(t => t.Id.Equals(DatabaseSeed.ExplorersTeam1Id)).Name);
        }

        [Fact]
        public async void SearchWithFiltering_Ok()
        {
            //Arrange
            var firstExplorersTeam = m_databaseFixture.DataContext.ExplorersTeams.First(t => t.Id.Equals(DatabaseSeed.ExplorersTeam1Id));
            //ACT
            var (_, idFiltered) = await m_repository.SearchAsync(
                new Pagination(),
                new Ordering(),
                new ExplorersTeamFilter
                {
                    SearchTerm = firstExplorersTeam.Id.ToString()
                });

            var (_, nameFiltered) = await m_repository.SearchAsync(
                new Pagination(),
                new Ordering(),
                new ExplorersTeamFilter
                {
                    SearchTerm = firstExplorersTeam.Name
                });
            //ASSERT
            Assert.Equal(firstExplorersTeam.Id, idFiltered.First().Id);
            Assert.Equal(firstExplorersTeam.Name, nameFiltered.First().Name);
            Assert.Single(nameFiltered);
        }

        [Fact]
        public async Task AddAndDeleteExplorersTeam_Ok()
        {
            //Arrange
            var id = Guid.NewGuid();
            var explorersTeam = new ExplorersTeam
            {
                Id = id,
                Name = "Test"
            };

            //Act
            await m_repository.CreateAsync(explorersTeam);

            //Assert
            var (_, explorersTeams) = await m_repository.SearchAsync(new Pagination(), new Ordering(), new ExplorersTeamFilter
            {
                SearchTerm = id.ToString()
            });
            Assert.Equal(id, explorersTeams.First().Id);
            Assert.Equal("Test", explorersTeams.First().Name);
            await m_repository.DeleteAsync(explorersTeam);
            var (_, emptyResponse) = await m_repository.SearchAsync(new Pagination(), new Ordering(), new ExplorersTeamFilter
            {
                SearchTerm = id.ToString()
            });
            Assert.Empty(emptyResponse);
        }

        [Fact]
        public async Task UpdateNewExplorersTeam_Ok()
        {
            //Arrange
            var id = Guid.NewGuid();
            var explorersTeam = new ExplorersTeam
            {
                Id = id,
                Name = "Test",
            };

            //Act
            await m_repository.CreateAsync(explorersTeam);
            var (_, explorersTeams) = await m_repository.SearchAsync(new Pagination(), new Ordering(), new ExplorersTeamFilter
            {
                SearchTerm = id.ToString()
            });
            explorersTeam.Name = "Modified";
            await m_repository.UpdateAsync(new List<ExplorersTeam> {explorersTeam});
            var (_, updatedResponse) = await m_repository.SearchAsync(new Pagination(), new Ordering(), new ExplorersTeamFilter
            {
                SearchTerm = id.ToString()
            });

            //Assert
            Assert.Equal(id, explorersTeams.First().Id);
            Assert.Equal("Modified", explorersTeams.First().Name);
            await m_repository.DeleteAsync(updatedResponse.First());
        }
    }
}