using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Solaris.Web.CrewApi.Core.Models.Entities;
using Solaris.Web.CrewApi.Core.Models.Helpers.Commons;
using Solaris.Web.CrewApi.Core.Models.Interfaces.Filters;

namespace Solaris.Web.CrewApi.Core.Services.Interfaces
{
    public interface IShuttleService
    {
        Task CreateShuttleAsync(Shuttle shuttle);
        Task UpdateShuttleAsync(Shuttle shuttle);
        Task DeleteShuttleAsync(Guid id);
        Task<Tuple<int, List<Shuttle>>> SearchShuttleAsync(Pagination pagination, Ordering ordering, IFilter<Shuttle> filter);
    }
}