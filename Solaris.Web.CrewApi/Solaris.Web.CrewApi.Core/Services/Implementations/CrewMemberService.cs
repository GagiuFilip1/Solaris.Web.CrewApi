using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Solaris.Web.CrewApi.Core.Models.Entities;
using Solaris.Web.CrewApi.Core.Models.Filters.Interfaces;
using Solaris.Web.CrewApi.Core.Models.Helpers.Commons;
using Solaris.Web.CrewApi.Core.Repositories.Interfaces;
using Solaris.Web.CrewApi.Core.Services.Interfaces;

namespace Solaris.Web.CrewApi.Core.Services.Implementations
{
    public class CrewMemberService : ICrewMemberService
    {
        private readonly ILogger<CrewMemberService> m_logger;
        private readonly ICrewMemberRepository m_crewMemberRepository;

        public CrewMemberService(ILogger<CrewMemberService> logger, ICrewMemberRepository crewMemberRepository)
        {
            m_logger = logger;
            m_crewMemberRepository = crewMemberRepository;
        }

        public async Task<Tuple<int, List<CrewMember>>> SearchCrewMemberAsync(Pagination pagination, Ordering ordering, IFilter<CrewMember> filtering)
        {
            try
            {
                return await m_crewMemberRepository.SearchAsync(pagination, ordering, filtering);
            }
            catch (Exception e)
            {
                m_logger.LogCritical(e, "Unexpected Exception while trying to search for CrewMembers");
                throw;
            }
        }

        public async Task<int> SimpleCountAsync(IFilter<CrewMember> filter)
        {
            try
            {
                return await m_crewMemberRepository.CountAsync(filter);
            }
            catch (Exception e)
            {
                m_logger.LogCritical(e, "Unexpected Exception while trying to count for CrewMembers");
                throw;
            }
        }
    }
}