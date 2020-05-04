using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Solaris.Web.CrewApi.Core.Enums;
using Solaris.Web.CrewApi.Core.Models.Interfaces;
using Solaris.Web.CrewApi.Core.Models.Interfaces.Commons;

namespace Solaris.Web.CrewApi.Core.Models.Entities
{
    public abstract class CrewMember : IIdentifier
    {
        [Key] public Guid Id { get; set; }

        [Column(TypeName = "varchar(256)")] public string Name { get; set; }
        
        public CrewMemberType Type { get; set; }
        
        public Guid ExplorersTeamId { get; set; }
        
        public ExplorersTeam ExplorersTeam { get; set; }
    }
}