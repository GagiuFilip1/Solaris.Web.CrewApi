using System.Threading.Tasks;
using Solaris.Web.CrewApi.Core.Enums;
using Solaris.Web.CrewApi.Core.Models.Rabbit.Helpers.Responses;

namespace Solaris.Web.CrewApi.Core.Models.Rabbit.Interfaces
{
    public interface IProcessor
    {
        public MessageType Type { get; set; }
        public Task<RabbitResponse> ProcessAsync(string data);
    }
}