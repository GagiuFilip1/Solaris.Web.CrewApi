using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Solaris.Web.CrewApi.Core.Models.Entities;
using Solaris.Web.CrewApi.Core.Models.Helpers.Commons;
using Solaris.Web.CrewApi.Core.Repositories.Interfaces;
using Solaris.Web.CrewApi.Infrastructure.Filters;
using Solaris.Web.CrewApi.Infrastructure.Repositories.Implementations;
using Solaris.Web.CrewApi.Tests.Utils;
using Xunit;

namespace Solaris.Web.CrewApi.Tests.RepositoryTests
{
    public class RobotRepositoryTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture m_databaseFixture;
        private readonly IRobotRepository m_repository;

        public RobotRepositoryTests(DatabaseFixture databaseFixture)
        {
            m_databaseFixture = databaseFixture;
            m_repository = new RobotRepository(m_databaseFixture.DataContext);
        }

        [Fact]
        public async void SimpleSearch_Ok()
        {
            //ACT
            var (count, robots) = await m_repository.SearchAsync(new Pagination(), new Ordering(), new RobotFilter());
            //ASSERT
            Assert.Equal(3, count);
            Assert.Equal(DatabaseSeed.RobotFirstTeam1Id, robots.First(t => t.Id.Equals(DatabaseSeed.RobotFirstTeam1Id)).Id);
            Assert.Equal(DatabaseSeed.RobotFirstTeam2Id, robots.First(t => t.Id.Equals(DatabaseSeed.RobotFirstTeam2Id)).Id);
            Assert.Equal(DatabaseSeed.RobotSecondTeam1Id, robots.First(t => t.Id.Equals(DatabaseSeed.RobotSecondTeam1Id)).Id);
        }

        [Fact]
        public async void SearchWithPagination_Ok()
        {
            //ACT
            var (countNoOffset, robotsNoOffset) = await m_repository.SearchAsync(new Pagination
            {
                Take = 1,
                Offset = 0
            }, new Ordering(), new RobotFilter());

            var (countWithOffset, robotsOffset) = await m_repository.SearchAsync(new Pagination
            {
                Take = 1,
                Offset = 1
            }, new Ordering(), new RobotFilter());

            //ASSERT
            Assert.Equal(3, countNoOffset);
            Assert.Equal(3, countWithOffset);
            Assert.Single((IEnumerable) robotsOffset);
            Assert.Single((IEnumerable) robotsNoOffset);
        }

        [Fact]
        public async void SearchWithOrdering_Ok()
        {
            //ACT
            var (_, descResult) = await m_repository.SearchAsync(
                new Pagination(),
                new Ordering
                {
                    OrderBy = nameof(Robot.Name),
                    OrderDirection = OrderDirection.Desc
                },
                new RobotFilter());

            var (_, ascResult) = await m_repository.SearchAsync(
                new Pagination(),
                new Ordering
                {
                    OrderBy = nameof(Robot.Name),
                    OrderDirection = OrderDirection.Asc
                },
                new RobotFilter());
            //ASSERT

            Assert.Equal(descResult.Count, ascResult.Count);
            Assert.Equal(descResult.First().Id, ascResult.Last().Id);
            Assert.Equal(descResult.First().Name, m_databaseFixture.DataContext.Robots.First(t => t.Id.Equals(DatabaseSeed.RobotSecondTeam1Id)).Name);
            Assert.Equal(ascResult.First().Name, m_databaseFixture.DataContext.Robots.First(t => t.Id.Equals(DatabaseSeed.RobotFirstTeam2Id)).Name);
        }

        [Fact]
        public async void SearchWithFiltering_Ok()
        {
            //Arrange
            var firstRobot = m_databaseFixture.DataContext.Robots.First(t => t.Id.Equals(DatabaseSeed.RobotFirstTeam1Id));
            //ACT
            var (_, idFiltered) = await m_repository.SearchAsync(
                new Pagination(),
                new Ordering(),
                new RobotFilter
                {
                    SearchTerm = firstRobot.Id.ToString()
                });

            var (_, nameFiltered) = await m_repository.SearchAsync(
                new Pagination(),
                new Ordering(),
                new RobotFilter
                {
                    SearchTerm = firstRobot.Name
                });
            //ASSERT
            Assert.Equal(firstRobot.Id, idFiltered.First().Id);
            Assert.Equal(firstRobot.Name, nameFiltered.First().Name);
            Assert.Single((IEnumerable) nameFiltered);
        }

        [Fact]
        public async Task AddAndDeleteNewRobot_Ok()
        {
            //Arrange
            var id = Guid.NewGuid();
            var robot = new Robot
            {
                Id = id,
                Name = "Test"
            };

            //Act
            await m_repository.CreateAsync(robot);

            //Assert
            var (_, robots) = await m_repository.SearchAsync(new Pagination(), new Ordering(), new RobotFilter
            {
                SearchTerm = id.ToString()
            });
            Assert.Equal(id, robots.First().Id);
            Assert.Equal("Test", robots.First().Name);
            await m_repository.DeleteAsync(robot);
            var (_, emptyResponse) = await m_repository.SearchAsync(new Pagination(), new Ordering(), new RobotFilter
            {
                SearchTerm = id.ToString()
            });
            Assert.Empty(emptyResponse);
        }

        [Fact]
        public async Task UpdateNewRobot_Ok()
        {
            //Arrange
            var id = Guid.NewGuid();
            var robot = new Robot
            {
                Id = id,
                Name = "Test",
            };

            //Act
            await m_repository.CreateAsync(robot);
            var (_, robots) = await m_repository.SearchAsync(new Pagination(), new Ordering(), new RobotFilter
            {
                SearchTerm = id.ToString()
            });
            robot.Name = "Modified";
            await m_repository.UpdateAsync(new List<Robot> {robot});
            var (_, updatedResponse) = await m_repository.SearchAsync(new Pagination(), new Ordering(), new RobotFilter
            {
                SearchTerm = id.ToString()
            });

            //Assert
            Assert.Equal(id, robots.First().Id);
            Assert.Equal("Modified", robots.First().Name);
            await m_repository.DeleteAsync(updatedResponse.First());
        }
    }
}