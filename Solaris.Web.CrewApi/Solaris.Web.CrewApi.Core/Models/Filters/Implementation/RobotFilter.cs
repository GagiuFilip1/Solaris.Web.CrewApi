﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Solaris.Web.CrewApi.Core.Extensions;
using Solaris.Web.CrewApi.Core.Models.Entities;
using Solaris.Web.CrewApi.Core.Models.Filters.Interfaces;

namespace Solaris.Web.CrewApi.Core.Models.Filters.Implementation
{
    public class RobotFilter : IFilter<Robot>
    {
        public string SearchTerm { get; set; }
        public List<Guid> Ids { get; set; }

        public IQueryable<Robot> Filter(IQueryable<Robot> filterQuery)
        {
            if (!string.IsNullOrEmpty(SearchTerm))
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