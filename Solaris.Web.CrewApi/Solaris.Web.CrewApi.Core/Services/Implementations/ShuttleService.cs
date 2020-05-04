using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Solaris.Web.CrewApi.Core.Models.Entities;
using Solaris.Web.CrewApi.Core.Models.Filters.Implementation;
using Solaris.Web.CrewApi.Core.Models.Filters.Interfaces;
using Solaris.Web.CrewApi.Core.Models.Helpers.Commons;
using Solaris.Web.CrewApi.Core.Repositories.Interfaces;
using Solaris.Web.CrewApi.Core.Services.Interfaces;

namespace Solaris.Web.CrewApi.Core.Services.Implementations
{
    public class ShuttleService : IShuttleService
    {
        private readonly ILogger<ShuttleService> m_logger;
        private readonly IShuttleRepository m_repository;
        private readonly IExplorersTeamRepository m_explorersTeamRepository;

        public ShuttleService(ILogger<ShuttleService> logger, IShuttleRepository repository, IExplorersTeamRepository explorersTeamRepository)
        {
            m_logger = logger;
            m_repository = repository;
            m_explorersTeamRepository = explorersTeamRepository;
        }

        public async Task CreateShuttleAsync(Shuttle shuttle)
        {
            try
            {
                var validationError = shuttle.Validate();
                if (validationError.Any())
                    throw new ValidationException($"A validation exception was raised while trying to create a Shuttle : {JsonConvert.SerializeObject(validationError, Formatting.Indented)}");
                await CheckExplorersTeamExistAsync(shuttle.ExplorersTeamId);
                await CheckExplorersTeamHaveNoShuttleAsync(shuttle.ExplorersTeamId);
                await m_repository.CreateAsync(shuttle);
            }
            catch (ValidationException e)
            {
                m_logger.LogWarning(e, "A validation failed");
                throw;
            }
            catch (Exception e) when (e.GetType() != typeof(ValidationException))
            {
                m_logger.LogCritical(e, $"Unexpected Exception while trying to create a Shuttle with the properties : {JsonConvert.SerializeObject(shuttle, Formatting.Indented)}");
                throw;
            }
        }

        public async Task UpdateShuttleAsync(Shuttle shuttle)
        {
            try
            {
                var validationError = shuttle.Validate();
                if (validationError.Any())
                    throw new ValidationException($"A validation exception was raised while trying to update a Shuttle : {JsonConvert.SerializeObject(validationError, Formatting.Indented)}");
                await EnsureShuttleExistAsync(shuttle.Id);
                await CheckExplorersTeamExistAsync(shuttle.ExplorersTeamId);
                await m_repository.UpdateAsync(new List<Shuttle> {shuttle});
            }
            catch (ValidationException e)
            {
                m_logger.LogWarning(e, "A validation failed");
                throw;
            }
            catch (Exception e) when (e.GetType() != typeof(ValidationException))
            {
                m_logger.LogCritical(e, $"Unexpected Exception while trying to update a Shuttle with the properties : {JsonConvert.SerializeObject(shuttle, Formatting.Indented)}");
                throw;
            }
        }

        public async Task DeleteShuttleAsync(Guid id)
        {
            try
            {
                var shuttle = await EnsureShuttleExistAsync(id);
                await m_repository.DeleteAsync(shuttle);
            }
            catch (ValidationException e)
            {
                m_logger.LogWarning(e, "A validation failed");
                throw;
            }
            catch (Exception e) when (e.GetType() != typeof(ValidationException))
            {
                m_logger.LogCritical(e, $"Unexpected Exception while trying to delete a Shuttle for id : {id}");
                throw;
            }
        }

        public async Task<Tuple<int, List<Shuttle>>> SearchShuttleAsync(Pagination pagination, Ordering ordering, IFilter<Shuttle> filter)
        {
            try
            {
                return await m_repository.SearchAsync(pagination, ordering, filter);
            }
            catch (Exception e)
            {
                m_logger.LogCritical(e, "Unexpected Exception while trying to search for Shuttles");
                throw;
            }
        }

        private async Task<Shuttle> EnsureShuttleExistAsync(Guid id)
        {
            var (_, searchResult) = await m_repository.SearchAsync(new Pagination(), new Ordering(), new ShuttleFilter
            {
                SearchTerm = id.ToString()
            });

            if (!searchResult.Any())
                throw new ValidationException("No Shuttle was found for the specified Id");
            return searchResult.First();
        }

        private async Task CheckExplorersTeamExistAsync(Guid id)
        {
            var (_, searchResult) = await m_explorersTeamRepository.SearchAsync(new Pagination(), new Ordering(), new ExplorersTeamFilter
            {
                SearchTerm = id.ToString()
            });

            if (!searchResult.Any())
                throw new ValidationException("No Explorers Team was found for the specified Id");
        }

        private async Task CheckExplorersTeamHaveNoShuttleAsync(Guid explorersTeamId)
        {
            var (count, _) = await SearchShuttleAsync(new Pagination(), new Ordering(), new ShuttleFilter
            {
                SearchTerm = explorersTeamId.ToString()
            });
            if (count > 0)
                throw new ValidationException("There is already a shuttle assigned to the team");
        }
    }
}