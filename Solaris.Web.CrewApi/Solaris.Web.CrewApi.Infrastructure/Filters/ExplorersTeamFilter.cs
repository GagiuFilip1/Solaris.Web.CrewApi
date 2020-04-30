﻿using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Solaris.Web.CrewApi.Core.Extensions;
using Solaris.Web.CrewApi.Core.Models.Entities;
using Solaris.Web.CrewApi.Core.Models.Interfaces;

namespace Solaris.Web.CrewApi.Infrastructure.Filters
{
    public class ExplorersTeamFilter : IFilter<ExplorersTeam>
    {
        public string SearchTerm { get; set; }

        public IQueryable<ExplorersTeam> Filter(IQueryable<ExplorersTeam> filterQuery)
        {
            if (string.IsNullOrEmpty(SearchTerm))
                return filterQuery;

            filterQuery = Guid.TryParse(SearchTerm, out var guid)
                ? filterQuery.Where(p => p.Id.Equals(guid))
                : filterQuery.Where(p => EF.Functions.Like(p.Name, SearchTerm.ToMySqlLikeSyntax()));
            
            return filterQuery;
        }
    }
}