using System;
using System.Collections.Generic;
using Solaris.Web.CrewApi.Core.Enums;
using Solaris.Web.CrewApi.Core.Models.Entities;

namespace Solaris.Web.CrewApi.Tests.Utils
{
    public class DatabaseSeed
    {
        public static Guid ExplorersTeam1Id = Guid.NewGuid();
        public static Guid ExplorersTeam2Id = Guid.NewGuid();

        public static Guid Shuttle1Id = Guid.NewGuid();
        public static Guid Shuttle2Id = Guid.NewGuid();

        public static Guid Captain1Id = Guid.NewGuid();
        public static Guid Captain2Id = Guid.NewGuid();

        public static Guid RobotFirstTeam1Id = Guid.NewGuid();
        public static Guid RobotFirstTeam2Id = Guid.NewGuid();

        public static Guid RobotSecondTeam1Id = Guid.NewGuid();

        public static List<ExplorersTeam> GetExplorerTeams()
        {
            return new List<ExplorersTeam>
            {
                new ExplorersTeam
                {
                    Id = ExplorersTeam1Id,
                    Name = "DontKnowHowWeGotHere",
                    DepartedAt = DateTime.Now.AddYears(100)
                },
                new ExplorersTeam
                {
                    Id = ExplorersTeam2Id,
                    Name = "ToProductionAndBeyond",
                    DepartedAt = DateTime.Now.AddYears(120)
                }
            };
        }

        public static List<Shuttle> GetShuttles()
        {
            return new List<Shuttle>
            {
                new Shuttle
                {
                    Id = Shuttle1Id,
                    ShipNumber = "1A2B3C4D",
                    Name = "NeverGonnaGiveYouUp",
                    CreationDate = DateTime.Now.AddYears(90),
                    ExplorersTeamId = ExplorersTeam1Id
                },
                new Shuttle
                {
                    Id = Shuttle2Id,
                    ShipNumber = "A1B2C3D4",
                    Name = "NeverGonnaLetYouDown",
                    CreationDate = DateTime.Now.AddYears(100),
                    ExplorersTeamId = ExplorersTeam1Id
                }
            };
        }

        public static List<Captain> GetCaptains()
        {
            return new List<Captain>
            {
                new Captain
                {
                    Id = Captain1Id,
                    Age = 47,
                    Email = "1@email",
                    ExplorersTeamId = ExplorersTeam1Id,
                    Gender = HumanGender.Male,
                    Name = "Louis QuickRocket"
                },
                new Captain
                {
                    Id = Captain2Id,
                    Age = 41,
                    Email = "2@email",
                    ExplorersTeamId = ExplorersTeam2Id,
                    Gender = HumanGender.Female,
                    Name = "Ema LongShuttle"
                }
            };
        }

        public static List<Robot> GetRobots()
        {
            return new List<Robot>
            {
                new Robot
                {
                    Id = RobotFirstTeam1Id,
                    CreationDate = DateTime.Now.AddYears(90),
                    CurrentStatus = RobotStatus.Free,
                    ExplorersTeamId = ExplorersTeam1Id,
                    ProductNumber = "AB-1000",
                    Name = "SpareParty"
                },
                new Robot
                {
                    Id = RobotFirstTeam2Id,
                    CreationDate = DateTime.Now.AddYears(90),
                    CurrentStatus = RobotStatus.Free,
                    ExplorersTeamId = ExplorersTeam1Id,
                    ProductNumber = "AB-1000",
                    Name = "ProductionHotFix"
                },
                new Robot
                {
                    Id = RobotSecondTeam1Id,
                    CreationDate = DateTime.Now.AddYears(100),
                    CurrentStatus = RobotStatus.Free,
                    ExplorersTeamId = ExplorersTeam1Id,
                    ProductNumber = "AB-2000",
                    Name = "WhoNeedUnitTests"
                }
            };
        }
    }
}