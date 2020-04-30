﻿using System.Threading.Tasks;
using Solaris.Web.CrewApi.Core.Repositories.Interfaces;
using Solaris.Web.CrewApi.Infrastructure.Repositories.Implementations;
using Solaris.Web.CrewApi.Tests.Utils;
using Xunit;

namespace Solaris.Web.CrewApi.Tests.RepositoryTests
{
    public class ShuttleRepositoryTests
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

            //ASSERT
        }

        [Fact]
        public async void SearchWithPagination_Ok()
        {
            //ACT

            //ASSERT
        }

        [Fact]
        public async void SearchWithOrdering_Ok()
        {
            //ACT

            //ASSERT
        }

        [Fact]
        public async void SearchWithFiltering_Ok()
        {
            //Arrange


            //ACT


            //ASSERT
        }

        [Fact]
        public async Task AddAndDeleteNewSolarSystem_Ok()
        {
            //Arrange


            //Act


            //Assert
        }

        [Fact]
        public async Task UpdateNewPlanet_Ok()
        {
            //Arrange

            //Act


            //Assert
        }
    }
}