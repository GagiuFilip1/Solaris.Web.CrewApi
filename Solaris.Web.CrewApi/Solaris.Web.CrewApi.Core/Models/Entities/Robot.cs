using System;
using System.ComponentModel.DataAnnotations.Schema;
using Solaris.Web.CrewApi.Core.Enums;

namespace Solaris.Web.CrewApi.Core.Models.Entities
{
    public class Robot : CrewMember
    {
        [Column(TypeName = "varchar(256)")] public string ProductNumber { get; set; }

        public DateTime CreationDate { get; set; }

        public Guid CurrentPlanetId { get; set; }

        public RobotStatus CurrentStatus { get; set; }
    }
}