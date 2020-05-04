using Solaris.Web.CrewApi.Core.Models.Rabbit.Helpers.Setup;

namespace Solaris.Web.CrewApi.Core.Models.Helpers.Commons
{
    public class AppSettings
    {
        public RabbitMqSettings RabbitMq { get; set; }
        
        public RabbitMqQueues RabbitMqQueues { get; set; }
    }
}