using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Solaris.Web.CrewApi.Core.Models.Filters.Interfaces;
using Solaris.Web.CrewApi.Core.Models.Helpers.Commons;
using Solaris.Web.CrewApi.Core.Models.Interfaces.Commons;

namespace Solaris.Web.CrewApi.Core.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : IIdentifier
    {
        Task CreateAsync(TEntity entity);
        Task UpdateAsync(List<TEntity> entities);
        Task DeleteAsync(TEntity entity);
        Task<Tuple<int, List<TEntity>>> SearchAsync(Pagination pagination, Ordering ordering, IFilter<TEntity> filtering);
    }
}