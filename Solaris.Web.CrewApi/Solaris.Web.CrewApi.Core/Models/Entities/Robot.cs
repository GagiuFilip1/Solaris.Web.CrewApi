using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Solaris.Web.CrewApi.Core.Enums;
using Solaris.Web.CrewApi.Core.Extensions;
using Solaris.Web.CrewApi.Core.Models.Interfaces.Commons;

namespace Solaris.Web.CrewApi.Core.Models.Entities
{
    public class Robot : CrewMember, IValidEntity
    {
        [Column(TypeName = "varchar(256)")] public string ProductNumber { get; set; }

        public DateTime CreationDate { get; set; }

        public Guid? CurrentPlanetId { get; set; }

        public RobotStatus CurrentStatus { get; set; }

        public List<string> Validate()
        {
            var errors = new List<string>();
            if (!ProductNumber.HasOnlyNumbers())
                errors.Add("The product Number is invalid, must contain only numbers");
            
            return errors;
        }
    }
}