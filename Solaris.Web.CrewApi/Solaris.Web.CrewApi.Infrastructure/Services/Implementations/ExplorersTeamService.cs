using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Solaris.Web.CrewApi.Core.Models.Entities;
using Solaris.Web.CrewApi.Core.Models.Helpers;
using Solaris.Web.CrewApi.Core.Models.Interfaces;
using Solaris.Web.CrewApi.Core.Repositories.Interfaces;
using Solaris.Web.CrewApi.Core.Services.Interfaces;
using Solaris.Web.CrewApi.Infrastructure.Filters;
using Solaris.Web.CrewApi.Infrastructure.Ioc;

namespace Solaris.Web.CrewApi.Infrastructure.Services.Implementations
{
    [RegistrationKind(Type = RegistrationType.Scoped)]
    public class ExplorersTeamService : IExplorersTeamService
    {
        private readonly ILogger<ExplorersTeamService> m_logger;
        private readonly IExplorersTeamRepository m_repository;

        public ExplorersTeamService(ILogger<ExplorersTeamService> logger, IExplorersTeamRepository repository)
        {
            m_logger = logger;
            m_repository = repository;
        }

        public async Task CreateExplorersTeamAsync(ExplorersTeam explorersTeam)
        {
            try
            {
                var validationError = explorersTeam.Validate();
                if (validationError.Any())
                    throw new ValidationException($"A validation exception was raised while trying to create a ExplorersTeam : {JsonConvert.SerializeObject(validationError, Formatting.Indented)}");
                await m_repository.CreateAsync(explorersTeam);
            }
            catch (ValidationException e)
            {
                m_logger.LogWarning(e, "A validation failed");
                throw;
            }
            catch (Exception e) when (e.GetType() != typeof(ValidationException))
            {
                m_logger.LogCritical(e, $"Unexpected Exception while trying to create a ExplorersTeam with the properties : {JsonConvert.SerializeObject(explorersTeam, Formatting.Indented)}");
                throw;
            }
        }

        public async Task UpdateExplorersTeamAsync(ExplorersTeam explorersTeam)
        {
            try
            {
                var validationError = explorersTeam.Validate();
                if (validationError.Any())
                    throw new ValidationException($"A validation exception was raised while trying to update a ExplorersTeam : {JsonConvert.SerializeObject(validationError, Formatting.Indented)}");
                await EnsureExplorersTeamExistAsync(explorersTeam.Id);
                await m_repository.UpdateAsync(explorersTeam);
            }
            catch (ValidationException e)
            {
                m_logger.LogWarning(e, "A validation failed");
                throw;
            }
            catch (Exception e) when (e.GetType() != typeof(ValidationException))
            {
                m_logger.LogCritical(e, $"Unexpected Exception while trying to update a ExplorersTeam with the properties : {JsonConvert.SerializeObject(explorersTeam, Formatting.Indented)}");
                throw;
            }
        }

        public async Task DeleteExplorersTeamAsync(Guid id)
        {
            try
            {
                var explorersTeam = await EnsureExplorersTeamExistAsync(id);
                await m_repository.DeleteAsync(explorersTeam);
            }
            catch (ValidationException e)
            {
                m_logger.LogWarning(e, "A validation failed");
                throw;
            }
            catch (Exception e) when (e.GetType() != typeof(ValidationException))
            {
                m_logger.LogCritical(e, $"Unexpected Exception while trying to delete a ExplorersTeam for id : {id}");
                throw;
            }
        }

        public async Task<Tuple<int, List<ExplorersTeam>>> SearchExplorersTeamAsync(Pagination pagination, Ordering ordering, IFilter<ExplorersTeam> filter)
        {
            try
            {
                return await m_repository.SearchAsync(pagination, ordering, filter);
            }
            catch (Exception e)
            {
                m_logger.LogCritical(e, "Unexpected Exception while trying to search for ExplorersTeams");
                throw;
            }
        }

        private async Task<ExplorersTeam> EnsureExplorersTeamExistAsync(Guid id)
        {
            var (_, searchResult) = await m_repository.SearchAsync(new Pagination(), new Ordering(), new ExplorersTeamFilter
            {
                SearchTerm = id.ToString()
            });

            if (!searchResult.Any())
                throw new ValidationException("No ExplorersTeam was found for the specified Id");
            return searchResult.First();
        }
    }
}