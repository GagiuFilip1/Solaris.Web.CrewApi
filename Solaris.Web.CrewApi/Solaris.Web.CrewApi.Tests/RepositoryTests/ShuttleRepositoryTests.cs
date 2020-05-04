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
    public class ShuttleRepositoryTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture m_databaseFixture;
        private readonly IShuttleRepository m_repository;

        public ShuttleRepositoryTests(DatabaseFixture databaseFixture)
        {
            m_databaseFixture = databaseFixture;
            m_repository = new ShuttleRepository(m_databaseFixture.DataContext);
        }

        [Fact]
        public async void SimpleSearch_Ok()
        {
            //ACT
            var (count, shuttles) = await m_repository.SearchAsync(new Pagination(), new Ordering(), new ShuttleFilter());
            //ASSERT
            Assert.Equal(2, count);
            Assert.Equal(DatabaseSeed.Shuttle1Id, shuttles.First(t => t.Id.Equals(DatabaseSeed.Shuttle1Id)).Id);
            Assert.Equal(DatabaseSeed.Shuttle2Id, shuttles.First(t => t.Id.Equals(DatabaseSeed.Shuttle2Id)).Id);
        }

        [Fact]
        public async void SearchWithPagination_Ok()
        {
            //ACT
            var (countNoOffset, shuttlesNoOffset) = await m_repository.SearchAsync(new Pagination
            {
                Take = 1,
                Offset = 0
            }, new Ordering(), new ShuttleFilter());

            var (countWithOffset, shuttlesOffset) = await m_repository.SearchAsync(new Pagination
            {
                Take = 1,
                Offset = 1
            }, new Ordering(), new ShuttleFilter());

            //ASSERT
            Assert.Equal(2, countNoOffset);
            Assert.Equal(2, countWithOffset);
            Assert.Single((IEnumerable) shuttlesOffset);
            Assert.Single((IEnumerable) shuttlesNoOffset);
        }

        [Fact]
        public async void SearchWithOrdering_Ok()
        {
            //ACT
            var (_, descResult) = await m_repository.SearchAsync(
                new Pagination(),
                new Ordering
                {
                    OrderBy = nameof(Shuttle.Name),
                    OrderDirection = OrderDirection.Desc
                },
                new ShuttleFilter());

            var (_, ascResult) = await m_repository.SearchAsync(
                new Pagination(),
                new Ordering
                {
                    OrderBy = nameof(Shuttle.Name),
                    OrderDirection = OrderDirection.Asc
                },
                new ShuttleFilter());
            //ASSERT

            Assert.Equal(descResult.Count, ascResult.Count);
            Assert.Equal(descResult.First().Id, ascResult.Last().Id);
            Assert.Equal(descResult.First().Name, m_databaseFixture.DataContext.Shuttles.First(t => t.Id.Equals(DatabaseSeed.Shuttle2Id)).Name);
            Assert.Equal(ascResult.First().Name, m_databaseFixture.DataContext.Shuttles.First(t => t.Id.Equals(DatabaseSeed.Shuttle1Id)).Name);
        }

        [Fact]
        public async void SearchWithFiltering_Ok()
        {
            //Arrange
            var firstShuttle = m_databaseFixture.DataContext.Shuttles.First(t => t.Id.Equals(DatabaseSeed.Shuttle1Id));
            //ACT
            var (_, idFiltered) = await m_repository.SearchAsync(
                new Pagination(),
                new Ordering(),
                new ShuttleFilter
                {
                    SearchTerm = firstShuttle.Id.ToString()
                });

            var (_, nameFiltered) = await m_repository.SearchAsync(
                new Pagination(),
                new Ordering(),
                new ShuttleFilter
                {
                    SearchTerm = firstShuttle.Name
                });
            //ASSERT
            Assert.Equal(firstShuttle.Id, idFiltered.First().Id);
            Assert.Equal(firstShuttle.Name, nameFiltered.First().Name);
            Assert.Single(nameFiltered);
        }

        [Fact]
        public async Task AddAndDeleteNewShuttle_Ok()
        {
            //Arrange
            var id = Guid.NewGuid();
            var shuttle = new Shuttle
            {
                Id = id,
                Name = "Test"
            };

            //Act
            await m_repository.CreateAsync(shuttle);

            //Assert
            var (_, shuttles) = await m_repository.SearchAsync(new Pagination(), new Ordering(), new ShuttleFilter
            {
                SearchTerm = id.ToString()
            });
            Assert.Equal(id, shuttles.First().Id);
            Assert.Equal("Test", shuttles.First().Name);
            await m_repository.DeleteAsync(shuttle);
            var (_, emptyResponse) = await m_repository.SearchAsync(new Pagination(), new Ordering(), new ShuttleFilter
            {
                SearchTerm = id.ToString()
            });
            Assert.Empty(emptyResponse);
        }

        [Fact]
        public async Task UpdateNewShuttle_Ok()
        {
            //Arrange
            var id = Guid.NewGuid();
            var shuttle = new Shuttle
            {
                Id = id,
                Name = "Test",
            };

            //Act
            await m_repository.CreateAsync(shuttle);
            var (_, shuttles) = await m_repository.SearchAsync(new Pagination(), new Ordering(), new ShuttleFilter
            {
                SearchTerm = id.ToString()
            });
            shuttle.Name = "Modified";
            await m_repository.UpdateAsync(new List<Shuttle> {shuttle});
            var (_, updatedResponse) = await m_repository.SearchAsync(new Pagination(), new Ordering(), new ShuttleFilter
            {
                SearchTerm = id.ToString()
            });

            //Assert
            Assert.Equal(id, shuttles.First().Id);
            Assert.Equal("Modified", shuttles.First().Name);
            await m_repository.DeleteAsync(updatedResponse.First());
        }
    }
}