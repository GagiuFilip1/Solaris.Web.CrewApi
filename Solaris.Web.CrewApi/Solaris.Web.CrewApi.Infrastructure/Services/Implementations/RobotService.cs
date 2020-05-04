using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Solaris.Web.CrewApi.Core.Enums;
using Solaris.Web.CrewApi.Core.Models.Entities;
using Solaris.Web.CrewApi.Core.Models.Helpers.Commons;
using Solaris.Web.CrewApi.Core.Models.Helpers.Rabbit.Responses;
using Solaris.Web.CrewApi.Core.Models.Interfaces.Filters;
using Solaris.Web.CrewApi.Core.Repositories.Interfaces;
using Solaris.Web.CrewApi.Core.Services.Interfaces;
using Solaris.Web.CrewApi.Infrastructure.Filters;
using Solaris.Web.CrewApi.Infrastructure.Ioc;
using Solaris.Web.CrewApi.Infrastructure.Rabbit;

namespace Solaris.Web.CrewApi.Infrastructure.Services.Implementations
{
    [RegistrationKind(Type = RegistrationType.Scoped)]
    public class RobotService : IRobotService
    {
        private readonly ILogger<RobotService> m_logger;
        private readonly IRobotRepository m_repository;
        private readonly IExplorersTeamRepository m_explorersTeamRepository;
        private readonly AppSettings m_appSettings;
        private readonly RabbitHandler m_rabbitHandler;

        public RobotService(ILogger<RobotService> logger, IRobotRepository repository, IExplorersTeamRepository explorersTeamRepository, RabbitHandler rabbitHandler, IOptions<AppSettings> appSettings)
        {
            m_logger = logger;
            m_repository = repository;
            m_explorersTeamRepository = explorersTeamRepository;
            m_rabbitHandler = rabbitHandler;
            m_appSettings = appSettings.Value;
        }

        public async Task CreateRobotAsync(Robot robot)
        {
            try
            {
                var validationError = robot.Validate();
                if (validationError.Any())
                    throw new ValidationException($"A validation exception was raised while trying to create a Robot : {JsonConvert.SerializeObject(validationError, Formatting.Indented)}");
                await CheckExplorersTeamExistAsync(robot.ExplorersTeamId);
                CheckIfPlanetExist(robot.CurrentPlanetId);
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
                CheckIfPlanetExist(robot.CurrentPlanetId);
                await CheckExplorersTeamExistAsync(robot.ExplorersTeamId);
                await m_repository.UpdateAsync(new List<Robot> {robot});
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

        public async Task UpdateListOfRobotsAsync(List<Robot> robots)
        {
            foreach (var robot in robots)
            {
                var validationError = robot.Validate();
                if (validationError.Any())
                    throw new ValidationException($"A validation exception was raised while trying to update a Robot : {JsonConvert.SerializeObject(validationError, Formatting.Indented)}");
                await EnsureRobotExistAsync(robot.Id);
                CheckIfPlanetExist(robot.CurrentPlanetId);
                await CheckExplorersTeamExistAsync(robot.ExplorersTeamId);   
            }
            await m_repository.UpdateAsync(robots);
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

        public async Task SendRobotsToPlanetAsync(IFilter<Robot> filter, Guid planetId)
        {
            try
            {
                var (_, searchedRobots ) = await SearchRobotAsync(new Pagination(), new Ordering(), filter);
                var response = m_rabbitHandler.PublishRpc<RabbitResponse>(new PublishOptions
                {
                    TargetQueue = m_appSettings.RabbitMqQueues.SolarApiQueue,
                    Message = JsonConvert.SerializeObject(new
                    {
                        PlanetId = planetId,
                        Robots = searchedRobots
                    }),
                    Headers = new Dictionary<string, object>
                    {
                        {nameof(MessageType), nameof(MessageType.SendRobotsToPlanet)}
                    }
                });

                if (response.IsSuccessful)
                {
                    var (_, bots ) = await SearchRobotAsync(new Pagination(), new Ordering(), filter);
                    bots.ForEach(robot =>
                    {
                        robot.CurrentStatus = RobotStatus.Occupied;
                        robot.CurrentPlanetId = planetId;
                    });
                    await m_repository.UpdateAsync(bots);
                }
            }
            catch (ValidationException e)
            {
                m_logger.LogWarning(e, "A validation failed");
                throw;
            }
            catch (Exception e) when (e.GetType() != typeof(ValidationException))
            {
                m_logger.LogCritical(e, "Unexpected Exception while trying to send the robots to the planet");
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

        private void CheckIfPlanetExist(Guid? planetId)
        {
            if (!planetId.HasValue)
                return;

            var response = m_rabbitHandler.PublishRpc<RabbitResponse>(new PublishOptions
            {
                TargetQueue = m_appSettings.RabbitMqQueues.SolarApiQueue,
                Message = planetId.ToString(),
                Headers = new Dictionary<string, object>
                {
                    {nameof(MessageType), nameof(MessageType.CheckPlanet)}
                }
            });

            if (!response.IsSuccessful)
                throw new ValidationException("The Planet with he specified Id Does not exist");
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