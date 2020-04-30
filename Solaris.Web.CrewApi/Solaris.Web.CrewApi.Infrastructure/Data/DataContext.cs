using Microsoft.EntityFrameworkCore;
using Solaris.Web.CrewApi.Core.Enums;
using Solaris.Web.CrewApi.Core.Models.Entities;

namespace Solaris.Web.CrewApi.Infrastructure.Data
{
    public class DataContext : DbContext
    {
        public DbSet<ExplorersTeam> ExplorersTeams { get; set; }
        public DbSet<Shuttle> Shuttles { get; set; }
        public DbSet<CrewMember> CrewMembers { get; set; }
        public DbSet<Captain> Captains { get; set; }
        public DbSet<Robot> Robots { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetModelsRelations(modelBuilder);
            SetIndexes(modelBuilder);
            SetDescriminators(modelBuilder);
        }

        private static void SetModelsRelations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExplorersTeam>()
                .HasOne(t => t.Shuttle)
                .WithOne(t => t.ExplorersTeam)
                .HasForeignKey<Shuttle>(t => t.ExploreresTeamId);

            modelBuilder.Entity<CrewMember>()
                .HasOne(t => t.ExplorersTeam)
                .WithMany(t => t.CrewMembers);
        }

        private static void SetIndexes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExplorersTeam>()
                .HasIndex(t => t.Name)
                .IsUnique();

            modelBuilder.Entity<CrewMember>()
                .HasIndex(t => t.Name)
                .IsUnique();

            modelBuilder.Entity<Shuttle>()
                .HasIndex(t => t.Name)
                .IsUnique();
        }

        private static void SetDescriminators(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CrewMember>()
                .HasDiscriminator<CrewMemberType>("Type")
                .HasValue<Captain>(CrewMemberType.Human)
                .HasValue<Robot>(CrewMemberType.Robot);
        }
    }
}