﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Solaris.Web.CrewApi.Core.Models.Entities;
using Solaris.Web.CrewApi.Core.Models.Filters.Interfaces;
using Solaris.Web.CrewApi.Core.Models.Helpers.Commons;

namespace Solaris.Web.CrewApi.Core.Services.Interfaces
{
    public interface ICrewMemberService
    {
        Task<Tuple<int, List<CrewMember>>> SearchCrewMemberAsync(Pagination pagination, Ordering ordering, IFilter<CrewMember> filtering);
        Task<int> SimpleCountAsync(IFilter<CrewMember> filter);
    }
}