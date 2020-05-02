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
    public class ShuttleRepository : IShuttleRepository
    {
        private readonly DataContext m_dataContext;

        public ShuttleRepository(DataContext dataContext)
        {
            m_dataContext = dataContext;
        }

        public async Task CreateAsync(Shuttle entity)
        {
            await m_dataContext.Shuttles.AddAsync(entity);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(List<Shuttle> entities)
        {
            m_dataContext.Shuttles.UpdateRange(entities);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Shuttle entity)
        {
            m_dataContext.Shuttles.Remove(entity);
            await m_dataContext.SaveChangesAsync();
        }
        
        public async Task<Tuple<int, List<Shuttle>>> SearchAsync(Pagination pagination, Ordering ordering, IFilter<Shuttle> filtering)
        {
            return await filtering.Filter(m_dataContext.Shuttles.AsQueryable())
                .WithOrdering(ordering, new Ordering())
                .WithPaginationAsync(pagination);
        }
    }
}