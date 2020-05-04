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
    public class CaptainRepositoryTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture m_databaseFixture;
        private readonly ICaptainRepository m_repository;

        public CaptainRepositoryTests(DatabaseFixture databaseFixture)
        {
            m_databaseFixture = databaseFixture;
            m_repository = new CaptainRepository(m_databaseFixture.DataContext);
        }

        [Fact]
        public async void SimpleSearch_Ok()
        {
            //ACT
            var (count, captains) = await m_repository.SearchAsync(new Pagination(), new Ordering(), new CaptainFilter());
            //ASSERT
            Assert.Equal(2, count);
            Assert.Equal(DatabaseSeed.Captain1Id, captains.First(t => t.Id.Equals(DatabaseSeed.Captain1Id)).Id);
            Assert.Equal(DatabaseSeed.Captain2Id, captains.First(t => t.Id.Equals(DatabaseSeed.Captain2Id)).Id);
        }

        [Fact]
        public async void SearchWithPagination_Ok()
        {
            //ACT
            var (countNoOffset, captainsNoOffset) = await m_repository.SearchAsync(new Pagination
            {
                Take = 1,
                Offset = 0
            }, new Ordering(), new CaptainFilter());

            var (countWithOffset, captainsOffset) = await m_repository.SearchAsync(new Pagination
            {
                Take = 1,
                Offset = 1
            }, new Ordering(), new CaptainFilter());

            //ASSERT
            Assert.Equal(2, countNoOffset);
            Assert.Equal(2, countWithOffset);
            Assert.Single((IEnumerable) captainsOffset);
            Assert.Single((IEnumerable) captainsNoOffset);
        }

        [Fact]
        public async void SearchWithOrdering_Ok()
        {
            //ACT
            var (_, descResult) = await m_repository.SearchAsync(
                new Pagination(),
                new Ordering
                {
                    OrderBy = nameof(Captain.Name),
                    OrderDirection = OrderDirection.Desc
                },
                new CaptainFilter());

            var (_, ascResult) = await m_repository.SearchAsync(
                new Pagination(),
                new Ordering
                {
                    OrderBy = nameof(Captain.Name),
                    OrderDirection = OrderDirection.Asc
                },
                new CaptainFilter());
            //ASSERT

            Assert.Equal(descResult.Count, ascResult.Count);
            Assert.Equal(descResult.First().Id, ascResult.Last().Id);
            Assert.Equal(descResult.First().Name, m_databaseFixture.DataContext.Captains.First(t => t.Id.Equals(DatabaseSeed.Captain1Id)).Name);
            Assert.Equal(ascResult.First().Name, m_databaseFixture.DataContext.Captains.First(t => t.Id.Equals(DatabaseSeed.Captain2Id)).Name);
        }

        [Fact]
        public async void SearchWithFiltering_Ok()
        {
            //Arrange
            var firstCaptain = m_databaseFixture.DataContext.Captains.First(t => t.Id.Equals(DatabaseSeed.Captain1Id));
            //ACT
            var (_, idFiltered) = await m_repository.SearchAsync(
                new Pagination(),
                new Ordering(),
                new CaptainFilter
                {
                    SearchTerm = firstCaptain.Id.ToString()
                });

            var (_, nameFiltered) = await m_repository.SearchAsync(
                new Pagination(),
                new Ordering(),
                new CaptainFilter
                {
                    SearchTerm = firstCaptain.Name
                });
            //ASSERT
            Assert.Equal(firstCaptain.Id, idFiltered.First().Id);
            Assert.Equal(firstCaptain.Name, nameFiltered.First().Name);
            Assert.Single(nameFiltered);
        }

        [Fact]
        public async Task AddAndDeleteNewCaptain_Ok()
        {
            //Arrange
            var id = Guid.NewGuid();
            var captain = new Captain
            {
                Id = id,
                Name = "Test"
            };

            //Act
            await m_repository.CreateAsync(captain);

            //Assert
            var (_, captains) = await m_repository.SearchAsync(new Pagination(), new Ordering(), new CaptainFilter
            {
                SearchTerm = id.ToString()
            });
            Assert.Equal(id, captains.First().Id);
            Assert.Equal("Test", captains.First().Name);
            await m_repository.DeleteAsync(captain);
            var (_, emptyResponse) = await m_repository.SearchAsync(new Pagination(), new Ordering(), new CaptainFilter
            {
                SearchTerm = id.ToString()
            });
            Assert.Empty(emptyResponse);
        }

        [Fact]
        public async Task UpdateNewCaptain_Ok()
        {
            //Arrange
            var id = Guid.NewGuid();
            var captain = new Captain
            {
                Id = id,
                Name = "Test",
            };

            //Act
            await m_repository.CreateAsync(captain);
            var (_, captains) = await m_repository.SearchAsync(new Pagination(), new Ordering(), new CaptainFilter
            {
                SearchTerm = id.ToString()
            });
            captain.Name = "Modified";
            await m_repository.UpdateAsync(new List<Captain>{ captain });

        var (_, updatedResponse) = await m_repository.SearchAsync(new Pagination(), new Ordering(), new CaptainFilter
            {
                SearchTerm = id.ToString()
            });

            //Assert
            Assert.Equal(id, captains.First().Id);
            Assert.Equal("Modified", captains.First().Name);
            await m_repository.DeleteAsync(updatedResponse.First());
        }
    }
}