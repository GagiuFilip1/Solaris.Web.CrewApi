using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Solaris.Web.CrewApi.Core.Models.Interfaces;

namespace Solaris.Web.CrewApi.Core.Models.Entities
{
    public class ExplorersTeam : IIdentifier
    {
        public Guid Id { get; set; }

        [Column(TypeName = "varchar(256)")] public string Name { get; set; }

        public DateTime DepartedAt { get; set; }
        
        public Shuttle Shuttle { get; set; }
        
        public List<CrewMember> CrewMembers { get; set; }
    }
}