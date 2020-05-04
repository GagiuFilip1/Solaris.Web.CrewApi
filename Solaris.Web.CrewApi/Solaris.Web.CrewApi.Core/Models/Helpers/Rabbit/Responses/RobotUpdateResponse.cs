using System.Collections.Generic;
using Solaris.Web.CrewApi.Core.Models.Entities;

namespace Solaris.Web.CrewApi.Core.Models.Helpers.Rabbit.Responses
{
    public class RobotUpdateResponse
    {
        public List<Robot> Robots { get; set; }
    }
}