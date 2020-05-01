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
    public class CaptainService : ICaptainService
    {
        private readonly ILogger<CaptainService> m_logger;
        private readonly ICaptainRepository m_repository;
        private readonly IExplorersTeamRepository m_explorersTeamRepository;

        public CaptainService(ILogger<CaptainService> logger, ICaptainRepository repository, IExplorersTeamRepository explorersTeamRepository)
        {
            m_logger = logger;
            m_repository = repository;
            m_explorersTeamRepository = explorersTeamRepository;
        }

        public async Task CreateCaptainAsync(Captain captain)
        {
            try
            {
                var validationError = captain.Validate();
                if (validationError.Any())
                    throw new ValidationException($"A validation exception was raised while trying to create a Captain : {JsonConvert.SerializeObject(validationError, Formatting.Indented)}");
                await CheckExplorersTeamExistAsync(captain.ExplorersTeamId);
                await CheckExplorersTeamHaveNoCaptainAsync(captain.ExplorersTeamId);
                await m_repository.CreateAsync(captain);
            }
            catch (ValidationException e)
            {
                m_logger.LogWarning(e, "A validation failed");
                throw;
            }
            catch (Exception e) when (e.GetType() != typeof(ValidationException))
            {
                m_logger.LogCritical(e, $"Unexpected Exception while trying to create a Captain with the properties : {JsonConvert.SerializeObject(captain, Formatting.Indented)}");
                throw;
            }
        }

        public async Task UpdateCaptainAsync(Captain captain)
        {
            try
            {
                var validationError = captain.Validate();
                if (validationError.Any())
                    throw new ValidationException($"A validation exception was raised while trying to update a Captain : {JsonConvert.SerializeObject(validationError, Formatting.Indented)}");
                await EnsureCaptainExistAsync(captain.Id);
                await CheckExplorersTeamExistAsync(captain.ExplorersTeamId);
                await m_repository.UpdateAsync(captain);
            }
            catch (ValidationException e)
            {
                m_logger.LogWarning(e, "A validation failed");
                throw;
            }
            catch (Exception e) when (e.GetType() != typeof(ValidationException))
            {
                m_logger.LogCritical(e, $"Unexpected Exception while trying to update a Captain with the properties : {JsonConvert.SerializeObject(captain, Formatting.Indented)}");
                throw;
            }
        }

        public async Task DeleteCaptainAsync(Guid id)
        {
            try
            {
                var captain = await EnsureCaptainExistAsync(id);
                await m_repository.DeleteAsync(captain);
            }
            catch (ValidationException e)
            {
                m_logger.LogWarning(e, "A validation failed");
                throw;
            }
            catch (Exception e) when (e.GetType() != typeof(ValidationException))
            {
                m_logger.LogCritical(e, $"Unexpected Exception while trying to delete a Captain for id : {id}");
                throw;
            }
        }

        public async Task<Tuple<int, List<Captain>>> SearchCaptainAsync(Pagination pagination, Ordering ordering, IFilter<Captain> filter)
        {
            try
            {
                return await m_repository.SearchAsync(pagination, ordering, filter);
            }
            catch (Exception e)
            {
                m_logger.LogCritical(e, "Unexpected Exception while trying to search for Captains");
                throw;
            }
        }

        private async Task<Captain> EnsureCaptainExistAsync(Guid id)
        {
            var (_, searchResult) = await m_repository.SearchAsync(new Pagination(), new Ordering(), new CaptainFilter
            {
                SearchTerm = id.ToString()
            });

            if (!searchResult.Any())
                throw new ValidationException("No Captain was found for the specified Id");
            return searchResult.First();
        }

        private async Task CheckExplorersTeamExistAsync(Guid id)
        {
            var (_, searchResult) = await m_explorersTeamRepository.SearchAsync(new Pagination(), new Ordering(), new ExplorersTeamFilter
            {
                SearchTerm = id.ToString()
            });

            if (!searchResult.Any())
                throw new ValidationException("No Explorers team was found for the specified Id");
        }
        
        private async Task CheckExplorersTeamHaveNoCaptainAsync(Guid explorersTeamId)
        {
            var (count, _) = await SearchCaptainAsync(new Pagination(), new Ordering(), new CaptainFilter
            {
                SearchTerm = explorersTeamId.ToString()
            });

            if (count != 0)
                throw new ValidationException("The Explorers Team already have a captain assigned");
        }
    }
}