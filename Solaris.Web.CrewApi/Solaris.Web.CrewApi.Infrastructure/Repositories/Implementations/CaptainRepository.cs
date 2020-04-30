using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Solaris.Web.CrewApi.Core.Extensions;
using Solaris.Web.CrewApi.Core.Models.Entities;
using Solaris.Web.CrewApi.Core.Models.Helpers;
using Solaris.Web.CrewApi.Core.Models.Interfaces;
using Solaris.Web.CrewApi.Core.Repositories.Interfaces;
using Solaris.Web.CrewApi.Infrastructure.Data;
using Solaris.Web.CrewApi.Infrastructure.Ioc;

namespace Solaris.Web.CrewApi.Infrastructure.Repositories.Implementations
{
    [RegistrationKind(Type = RegistrationType.Scoped)]
    public class CaptainRepository : ICaptainRepository
    {
        private readonly DataContext m_dataContext;

        public CaptainRepository(DataContext dataContext)
        {
            m_dataContext = dataContext;
        }

        public async Task CreateAsync(Captain entity)
        {
            await m_dataContext.Captains.AddAsync(entity);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Captain entity)
        {
            m_dataContext.Captains.Update(entity);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Captain entity)
        {
            m_dataContext.Captains.Remove(entity);
            await m_dataContext.SaveChangesAsync();
        }
        
        public async Task<Tuple<int, List<Captain>>> SearchAsync(Pagination pagination, Ordering ordering, IFilter<Captain> filtering)
        {
            return await filtering.Filter(m_dataContext.Captains.AsQueryable())
                .WithOrdering(ordering, new Ordering())
                .WithPaginationAsync(pagination);
        }
    }
}