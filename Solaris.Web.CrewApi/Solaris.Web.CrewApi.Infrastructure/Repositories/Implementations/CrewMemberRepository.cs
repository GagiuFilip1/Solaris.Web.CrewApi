using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Solaris.Web.CrewApi.Core.Extensions;
using Solaris.Web.CrewApi.Core.Models.Entities;
using Solaris.Web.CrewApi.Core.Models.Helpers;
using Solaris.Web.CrewApi.Core.Models.Interfaces;
using Solaris.Web.CrewApi.Core.Repositories.Interfaces;
using Solaris.Web.CrewApi.Infrastructure.Data;

namespace Solaris.Web.CrewApi.Infrastructure.Repositories.Implementations
{
    public class CrewMemberRepository : ICrewMemberRepository
    {
        private readonly DataContext m_dataContext;

        public CrewMemberRepository(DataContext dataContext)
        {
            m_dataContext = dataContext;
        }

        public async Task<Tuple<int, List<CrewMember>>> SearchAsync(Pagination pagination, Ordering ordering, IFilter<CrewMember> filtering)
        {
            return await filtering.Filter(m_dataContext.CrewMembers.AsQueryable())
                .WithOrdering(ordering, new Ordering())
                .WithPaginationAsync(pagination);
        }

        public async Task<int> CountAsync(IFilter<CrewMember> filtering)
        {
            return await filtering.Filter(m_dataContext.CrewMembers.AsQueryable())
                .CountAsync();
        }
    }
}