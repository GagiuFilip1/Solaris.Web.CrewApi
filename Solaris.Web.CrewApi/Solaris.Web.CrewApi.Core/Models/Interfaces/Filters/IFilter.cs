using System;
using System.Collections.Generic;
using System.Linq;

namespace Solaris.Web.CrewApi.Core.Models.Interfaces.Filters
{
    public interface IFilter<TEntity> where TEntity : IIdentifier
    {
        public string SearchTerm { get; set; }
        public List<Guid> Ids { get; set; }
        IQueryable<TEntity> Filter(IQueryable<TEntity> filterQuery);
    }
}