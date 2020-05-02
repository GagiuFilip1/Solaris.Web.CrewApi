using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Solaris.Web.CrewApi.Core.Extensions;
using Solaris.Web.CrewApi.Core.Models.Entities;
using Solaris.Web.CrewApi.Core.Models.Interfaces.Filters;

namespace Solaris.Web.CrewApi.Infrastructure.Filters
{
    public class ShuttleFilter : IFilter<Shuttle>
    {
        public string SearchTerm { get; set; }
        public List<Guid> Ids { get; set; }

        public IQueryable<Shuttle> Filter(IQueryable<Shuttle> filterQuery)
        {
            if (string.IsNullOrEmpty(SearchTerm))
                return filterQuery;

            filterQuery = Guid.TryParse(SearchTerm, out var guid)
                ? filterQuery.Where(p => p.Id.Equals(guid) || p.ExplorersTeamId.Equals(guid))
                : filterQuery.Where(p => EF.Functions.Like(p.Name, SearchTerm.ToMySqlLikeSyntax()));
            
            if (Ids == null || !Ids.Any())
                return filterQuery;
            
            Ids.ForEach(t => { filterQuery = filterQuery.Where(p => p.Id.Equals(t) || p.ExplorersTeamId.Equals(t)); });
            return filterQuery;
        }
    }
}