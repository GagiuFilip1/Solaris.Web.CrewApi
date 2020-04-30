using Solaris.Web.CrewApi.Core.Enums;

namespace Solaris.Web.CrewApi.Core.Models.Entities
{
    public class Captain : CrewMember
    {
        public HumanGender Gender { get; set; }
        
        public int Age { get; set; }
        
        public string Email { get; set; }
    }
}