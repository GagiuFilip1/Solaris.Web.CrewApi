namespace Solaris.Web.CrewApi.Core.Models.Helpers
{
    public class AppSettings
    {
        public RabbitMqSettings RabbitMq { get; set; }
        
        public RabbitMqQueues RabbitMqQueues { get; set; }
    }
}