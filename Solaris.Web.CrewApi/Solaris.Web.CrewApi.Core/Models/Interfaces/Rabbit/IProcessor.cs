using System.Threading.Tasks;
using Solaris.Web.CrewApi.Core.Enums;
using Solaris.Web.CrewApi.Core.Models.Helpers.Rabbit.Responses;

namespace Solaris.Web.CrewApi.Core.Models.Interfaces.Rabbit
{
    public interface IProcessor
    {
        public MessageType Type { get; set; }
        public Task<RabbitResponse> ProcessAsync(string data);
    }
}