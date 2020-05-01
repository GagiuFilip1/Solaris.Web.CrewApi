﻿using System;
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
    public class RobotService : IRobotService
    {
        private readonly ILogger<RobotService> m_logger;
        private readonly IRobotRepository m_repository;
        private readonly IExplorersTeamRepository m_explorersTeamRepository;

        public RobotService(ILogger<RobotService> logger, IRobotRepository repository, IExplorersTeamRepository explorersTeamRepository)
        {
            m_logger = logger;
            m_repository = repository;
            m_explorersTeamRepository = explorersTeamRepository;
        }

        public async Task CreateRobotAsync(Robot robot)
        {
            try
            {
                var validationError = robot.Validate();
                if (validationError.Any())
                    throw new ValidationException($"A validation exception was raised while trying to create a Robot : {JsonConvert.SerializeObject(validationError, Formatting.Indented)}");
                await CheckExplorersTeamExistAsync(robot.ExplorersTeamId);
                await m_repository.CreateAsync(robot);
            }
            catch (ValidationException e)
            {
                m_logger.LogWarning(e, "A validation failed");
                throw;
            }
            catch (Exception e) when (e.GetType() != typeof(ValidationException))
            {
                m_logger.LogCritical(e, $"Unexpected Exception while trying to create a Robot with the properties : {JsonConvert.SerializeObject(robot, Formatting.Indented)}");
                throw;
            }
        }

        public async Task UpdateRobotAsync(Robot robot)
        {
            try
            {
                var validationError = robot.Validate();
                if (validationError.Any())
                    throw new ValidationException($"A validation exception was raised while trying to update a Robot : {JsonConvert.SerializeObject(validationError, Formatting.Indented)}");
                await EnsureRobotExistAsync(robot.Id);
                await CheckExplorersTeamExistAsync(robot.ExplorersTeamId);
                await m_repository.UpdateAsync(robot);
            }
            catch (ValidationException e)
            {
                m_logger.LogWarning(e, "A validation failed");
                throw;
            }
            catch (Exception e) when (e.GetType() != typeof(ValidationException))
            {
                m_logger.LogCritical(e, $"Unexpected Exception while trying to update a Robot with the properties : {JsonConvert.SerializeObject(robot, Formatting.Indented)}");
                throw;
            }
        }

        public async Task DeleteRobotAsync(Guid id)
        {
            try
            {
                var robot = await EnsureRobotExistAsync(id);
                await m_repository.DeleteAsync(robot);
            }
            catch (ValidationException e)
            {
                m_logger.LogWarning(e, "A validation failed");
                throw;
            }
            catch (Exception e) when (e.GetType() != typeof(ValidationException))
            {
                m_logger.LogCritical(e, $"Unexpected Exception while trying to delete a Robot for id : {id}");
                throw;
            }
        }

        public async Task<Tuple<int, List<Robot>>> SearchRobotAsync(Pagination pagination, Ordering ordering, IFilter<Robot> filter)
        {
            try
            {
                return await m_repository.SearchAsync(pagination, ordering, filter);
            }
            catch (Exception e)
            {
                m_logger.LogCritical(e, "Unexpected Exception while trying to search for Robots");
                throw;
            }
        }

        private async Task<Robot> EnsureRobotExistAsync(Guid id)
        {
            var (_, searchResult) = await m_repository.SearchAsync(new Pagination(), new Ordering(), new RobotFilter
            {
                SearchTerm = id.ToString()
            });

            if (!searchResult.Any())
                throw new ValidationException("No Robot was found for the specified Id");
            return searchResult.First();
        }

        private async Task CheckExplorersTeamExistAsync(Guid id)
        {
            var (_, searchResult) = await m_explorersTeamRepository.SearchAsync(new Pagination(), new Ordering(), new ExplorersTeamFilter
            {
                SearchTerm = id.ToString()
            });

            if (!searchResult.Any())
                throw new ValidationException("No Explorer team was found for the specified Id");
        }
    }
}