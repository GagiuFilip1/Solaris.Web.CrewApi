namespace Solaris.Web.CrewApi.Core.Models.Helpers.Rabbit.Setup
{
    public class QueueSetup
    {
        public string QueueName { get; set; }
        public ushort Qos { get; set; }
    }
}