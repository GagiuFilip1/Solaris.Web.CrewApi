using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Solaris.Web.CrewApi.Core.Enums;
using Solaris.Web.CrewApi.Core.Models.Filters.Implementation;
using Solaris.Web.CrewApi.Core.Models.Helpers.Commons;
using Solaris.Web.CrewApi.Core.Models.Rabbit.Helpers.Responses;
using Solaris.Web.CrewApi.Core.Models.Rabbit.Interfaces;
using Solaris.Web.CrewApi.Core.Services.Interfaces;
using Solaris.Web.CrewApi.Infrastructure.Ioc;

namespace Solaris.Web.CrewApi.Infrastructure.Rabbit.Processors
{
    [RegistrationKind(Type = RegistrationType.Scoped)]
    public class RobotsUpdateRequestProcess : IProcessor
    {
        private readonly IRobotService m_robotService;
        private readonly ILogger<RobotsUpdateRequestProcess> m_logger;

        public RobotsUpdateRequestProcess(IRobotService robotService, ILogger<RobotsUpdateRequestProcess> logger)
        {
            m_robotService = robotService;
            m_logger = logger;
        }

        public MessageType Type { get; set; } = MessageType.UpdateRobotStatus;

        public async Task<RabbitResponse> ProcessAsync(string data)
        {
            m_logger.LogInformation($"Received {data}");
            var response = JsonConvert.DeserializeObject<RobotUpdateResponse>(data);

            var (_, toUpdate) = await m_robotService.SearchRobotAsync(new Pagination(), new Ordering(), new RobotFilter
            {
                Ids = response.Robots.Select(t => t.Id).ToList()
            });
            response.Robots.ForEach(t =>
            {
                toUpdate.First(x => x.Id.Equals(t.Id)).CurrentStatus = t.CurrentStatus;
                toUpdate.First(x => x.Id.Equals(t.Id)).CurrentPlanetId = null;
            });
            await m_robotService.UpdateListOfRobotsAsync(toUpdate);
            return null;
        }
    }
}