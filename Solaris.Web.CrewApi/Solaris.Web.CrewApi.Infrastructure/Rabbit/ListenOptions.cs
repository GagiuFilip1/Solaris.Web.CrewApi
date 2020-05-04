using System;
using System.Threading.Tasks;
using Solaris.Web.CrewApi.Core.Enums;

namespace Solaris.Web.CrewApi.Infrastructure.Rabbit
{
    public class ListenOptions
    {
        public string TargetQueue { get; set; }
        public Func<string, Task> RequestParser { get; set; }
        public MessageType MessageType { get; set; }
        public ushort Qos { get; set; }
    }
}