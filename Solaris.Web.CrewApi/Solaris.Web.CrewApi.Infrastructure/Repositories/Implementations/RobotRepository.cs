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
    public class RobotRepository : IRobotRepository
    {
        private readonly DataContext m_dataContext;

        public RobotRepository(DataContext dataContext)
        {
            m_dataContext = dataContext;
        }

        public async Task CreateAsync(Robot entity)
        {
            await m_dataContext.Robots.AddAsync(entity);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(List<Robot> entities)
        {
            m_dataContext.Robots.UpdateRange(entities);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Robot entity)
        {
            m_dataContext.Robots.Remove(entity);
            await m_dataContext.SaveChangesAsync();
        }
        
        public async Task<Tuple<int, List<Robot>>> SearchAsync(Pagination pagination, Ordering ordering, IFilter<Robot> filtering)
        {
            return await filtering.Filter(m_dataContext.Robots.AsQueryable())
                .WithOrdering(ordering, new Ordering())
                .WithPaginationAsync(pagination);
        }
    }
}