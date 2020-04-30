using System;
using System.ComponentModel.DataAnnotations.Schema;
using Solaris.Web.CrewApi.Core.Models.Interfaces;

namespace Solaris.Web.CrewApi.Core.Models.Entities
{
    public class Shuttle : IIdentifier
    {
        public Guid Id { get; set; }

        [Column(TypeName = "varchar(16)")] public string ShipNumber { get; set; }

        [Column(TypeName = "varchar(256)")] public string Name { get; set; }

        public DateTime CreationDate { get; set; }

        public Guid ExploreresTeamId { get; set; }

        public ExplorersTeam ExplorersTeam { get; set; }
    }
}