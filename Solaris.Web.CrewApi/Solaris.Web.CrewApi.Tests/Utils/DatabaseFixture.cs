using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Solaris.Web.CrewApi.Infrastructure.Data;

namespace Solaris.Web.CrewApi.Tests.Utils
{
    public class DatabaseFixture : IDisposable
    {
        public DataContext DataContext { get; }

        public DatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            DataContext = new DataContext(options);
            SeedDataBase().Wait();
        }

        private async Task SeedDataBase()
        {
            var explorerTeams = DatabaseSeed.GetExplorerTeams();
            var shuttles = DatabaseSeed.GetShuttles();
            var captains = DatabaseSeed.GetCaptains();
            var robots = DatabaseSeed.GetRobots();
            
            await DataContext.AddRangeAsync(explorerTeams);
            await DataContext.AddRangeAsync(shuttles);
            await DataContext.AddRangeAsync(captains);
            await DataContext.AddRangeAsync(robots);
            await DataContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            DataContext.Database.EnsureDeleted();
            DataContext.Dispose();
        }
    }
}