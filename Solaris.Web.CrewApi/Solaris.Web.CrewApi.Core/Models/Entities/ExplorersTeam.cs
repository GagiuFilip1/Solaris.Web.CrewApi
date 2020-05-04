using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Solaris.Web.CrewApi.Core.Extensions;
using Solaris.Web.CrewApi.Core.Models.Interfaces.Commons;

namespace Solaris.Web.CrewApi.Core.Models.Entities
{
    public class ExplorersTeam : IIdentifier, IValidEntity
    {
        public Guid Id { get; set; }

        [Column(TypeName = "varchar(256)")] public string Name { get; set; }

        public DateTime DepartedAt { get; set; }
        
        public Shuttle Shuttle { get; set; }
        
        public List<CrewMember> CrewMembers { get; set; }
        
        public List<string> Validate()
        {
            var errors = new List<string>();
            if (!Name.HasOnlyLetters())
                errors.Add("The team Name must contain only letters");
            
            return errors;
        }
    }
}