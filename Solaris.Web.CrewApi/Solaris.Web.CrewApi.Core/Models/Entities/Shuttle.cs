using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Solaris.Web.CrewApi.Core.Extensions;
using Solaris.Web.CrewApi.Core.Models.Interfaces.Commons;

namespace Solaris.Web.CrewApi.Core.Models.Entities
{
    public class Shuttle : IIdentifier, IValidEntity
    {
        public Guid Id { get; set; }

        [Column(TypeName = "varchar(16)")] public string ShipNumber { get; set; }

        [Column(TypeName = "varchar(256)")] public string Name { get; set; }

        public DateTime CreationDate { get; set; }

        public Guid ExplorersTeamId { get; set; }

        public ExplorersTeam ExplorersTeam { get; set; }

        public List<string> Validate()
        {
            var errors = new List<string>();
            if (!ShipNumber.HasOnlyNumbersAndLetters())
                errors.Add("Ship Number must contain only numbers and letters");
            return errors;
        }
    }
}