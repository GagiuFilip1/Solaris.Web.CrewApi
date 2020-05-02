using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Solaris.Web.CrewApi.Core.Models.Entities;
using Solaris.Web.CrewApi.Core.Models.Helpers.Commons;
using Solaris.Web.CrewApi.Core.Models.Interfaces.Filters;

namespace Solaris.Web.CrewApi.Core.Services.Interfaces
{
    public interface ICaptainService
    {
        Task CreateCaptainAsync(Captain captain);
        Task UpdateCaptainAsync(Captain captain);
        Task DeleteCaptainAsync(Guid id);
        Task<Tuple<int, List<Captain>>> SearchCaptainAsync(Pagination pagination, Ordering ordering, IFilter<Captain> filter);
    }
}