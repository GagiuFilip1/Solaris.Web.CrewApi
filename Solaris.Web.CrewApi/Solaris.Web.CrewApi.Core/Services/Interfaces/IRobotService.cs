using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Solaris.Web.CrewApi.Core.Models.Entities;
using Solaris.Web.CrewApi.Core.Models.Helpers.Commons;
using Solaris.Web.CrewApi.Core.Models.Interfaces.Filters;

namespace Solaris.Web.CrewApi.Core.Services.Interfaces
{
    public interface IRobotService
    {
        Task CreateRobotAsync(Robot robot);
        Task UpdateRobotAsync(Robot robot);
        Task UpdateListOfRobotsAsync(List<Robot> robots);
        Task DeleteRobotAsync(Guid id);
        Task SendRobotsToPlanetAsync(IFilter<Robot> filter, Guid planetId);
        Task<Tuple<int, List<Robot>>> SearchRobotAsync(Pagination pagination, Ordering ordering, IFilter<Robot> filter);
    }
}