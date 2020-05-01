using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Solaris.Web.CrewApi.Core.Enums;
using Solaris.Web.CrewApi.Core.Extensions;
using Solaris.Web.CrewApi.Core.Models.Interfaces;

namespace Solaris.Web.CrewApi.Core.Models.Entities
{
    public class Captain : CrewMember, IValidEntity
    {
        public HumanGender Gender { get; set; }

        [Column(TypeName = "tinyint")] public int Age { get; set; }

        [Column(TypeName = "varchar(256)")] public string Email { get; set; }

        public List<string> Validate()
        {
            var errors = new List<string>();

            if (!Email.IsValidEmailFormat())
                errors.Add("Email is not valid");
            if (Age < 30 || Age > 65)
                errors.Add("A captain Age must be between 30 and 65 years");
            if (!Name.HasOnlyLetters())
                errors.Add("The captain Name must contains only letters");

            return errors;
        }
    }
}