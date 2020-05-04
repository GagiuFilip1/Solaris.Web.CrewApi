using System;
using System.Collections.Generic;
using System.Linq;
using Solaris.Web.CrewApi.Core.Models.Interfaces.Commons;

namespace Solaris.Web.CrewApi.Core.Models.Filters.Interfaces
{
    public interface IFilter<TEntity> where TEntity : IIdentifier
    {
        public string SearchTerm { get; set; }
        public List<Guid> Ids { get; set; }
        IQueryable<TEntity> Filter(IQueryable<TEntity> filterQuery);
    }
}