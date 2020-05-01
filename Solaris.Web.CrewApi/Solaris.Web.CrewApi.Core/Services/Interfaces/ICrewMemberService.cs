using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Solaris.Web.CrewApi.Core.Models.Entities;
using Solaris.Web.CrewApi.Core.Models.Helpers;
using Solaris.Web.CrewApi.Core.Models.Interfaces;

namespace Solaris.Web.CrewApi.Core.Services.Interfaces
{
    public interface ICrewMemberService
    {
        Task<Tuple<int, List<CrewMember>>> SearchCaptainAsync(Pagination pagination, Ordering ordering, IFilter<CrewMember> filtering);
        Task<int> SimpleCountAsync(IFilter<CrewMember> filter);
    }
}