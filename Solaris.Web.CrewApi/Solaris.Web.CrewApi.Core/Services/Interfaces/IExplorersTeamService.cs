using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Solaris.Web.CrewApi.Core.Models.Entities;
using Solaris.Web.CrewApi.Core.Models.Helpers.Commons;
using Solaris.Web.CrewApi.Core.Models.Interfaces.Filters;

namespace Solaris.Web.CrewApi.Core.Services.Interfaces
{
    public interface IExplorersTeamService
    {
        Task CreateExplorersTeamAsync(ExplorersTeam explorersTeam);
        Task UpdateExplorersTeamAsync(ExplorersTeam explorersTeam);
        Task DeleteExplorersTeamAsync(Guid id);
        Task<Tuple<int, List<ExplorersTeam>>> SearchExplorersTeamAsync(Pagination pagination, Ordering ordering, IFilter<ExplorersTeam> filter);
    }
}