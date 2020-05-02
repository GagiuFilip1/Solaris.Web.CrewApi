using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Solaris.Web.CrewApi.Core.Models.Helpers.Commons;
using Solaris.Web.CrewApi.Core.Models.Interfaces;
using Solaris.Web.CrewApi.Core.Models.Interfaces.Filters;

namespace Solaris.Web.CrewApi.Core.Repositories.Interfaces
{
    public interface ISimpleSearchRepository <TEntity> where TEntity : IIdentifier
    {
        Task<Tuple<int, List<TEntity>>> SearchAsync(Pagination pagination, Ordering ordering, IFilter<TEntity> filtering);
        Task<int> CountAsync(IFilter<TEntity> filtering);
    }
}