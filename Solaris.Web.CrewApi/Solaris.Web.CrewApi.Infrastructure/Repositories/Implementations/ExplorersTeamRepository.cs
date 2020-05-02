using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Solaris.Web.CrewApi.Core.Extensions;
using Solaris.Web.CrewApi.Core.Models.Entities;
using Solaris.Web.CrewApi.Core.Models.Helpers.Commons;
using Solaris.Web.CrewApi.Core.Models.Interfaces.Filters;
using Solaris.Web.CrewApi.Core.Repositories.Interfaces;
using Solaris.Web.CrewApi.Infrastructure.Data;
using Solaris.Web.CrewApi.Infrastructure.Ioc;

namespace Solaris.Web.CrewApi.Infrastructure.Repositories.Implementations
{
    [RegistrationKind(Type = RegistrationType.Scoped)]
    public class ExplorersTeamRepository : IExplorersTeamRepository
    {
        private readonly DataContext m_dataContext;

        public ExplorersTeamRepository(DataContext dataContext)
        {
            m_dataContext = dataContext;
        }

        public async Task CreateAsync(ExplorersTeam entity)
        {
            await m_dataContext.ExplorersTeams.AddAsync(entity);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(List<ExplorersTeam> entities)
        {
            m_dataContext.ExplorersTeams.UpdateRange(entities);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(ExplorersTeam entity)
        {
            m_dataContext.ExplorersTeams.Remove(entity);
            await m_dataContext.SaveChangesAsync();
        }
        
        public async Task<Tuple<int, List<ExplorersTeam>>> SearchAsync(Pagination pagination, Ordering ordering, IFilter<ExplorersTeam> filtering)
        {
            return await filtering.Filter(m_dataContext.ExplorersTeams.AsQueryable())
                .WithOrdering(ordering, new Ordering())
                .WithPaginationAsync(pagination);
        }
    }
}