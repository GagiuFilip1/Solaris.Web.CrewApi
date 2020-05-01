using System.Collections.Generic;

namespace Solaris.Web.CrewApi.Infrastructure.Rabbit
{
    public class RpcOptions
    {
        public string TargetQueue { get; set; }
        public Dictionary<string, object> Headers { get; set; }
        public string Message { get; set; }
    }
}