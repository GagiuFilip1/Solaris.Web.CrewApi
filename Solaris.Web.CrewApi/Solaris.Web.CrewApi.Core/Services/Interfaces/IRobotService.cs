using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Solaris.Web.CrewApi.Core.Models.Entities;
using Solaris.Web.CrewApi.Core.Models.Helpers;
using Solaris.Web.CrewApi.Core.Models.Interfaces;

namespace Solaris.Web.CrewApi.Core.Services.Interfaces
{
    public interface IRobotService
    {
        Task CreateRobotAsync(Robot robot);
        Task UpdateRobotAsync(Robot robot);
        Task DeleteRobotAsync(Guid id);
        Task<Tuple<int, List<Robot>>> SearchRobotAsync(Pagination pagination, Ordering ordering, IFilter<Robot> filter);
    }
}