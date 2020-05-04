using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Solaris.Web.CrewApi.Core.Enums;
using Solaris.Web.CrewApi.Core.Models.Rabbit.Helpers.Responses;
using Solaris.Web.CrewApi.Core.Models.Rabbit.Helpers.Setup;
using Solaris.Web.CrewApi.Core.Models.Rabbit.Models;

namespace Solaris.Web.CrewApi.Core.Models.Rabbit.Interfaces
{
    public interface IRabbitHandler
    {
        public Dictionary<MessageType, Func<string, Task<RabbitResponse>>> Processors { get; }
        T PublishRpc<T>(PublishOptions options);
        void PublishRpc(PublishOptions options);
        void Publish(PublishOptions options);
        void ListenQueueAsync(ListenOptions options);
        void DeclareRpcQueue(QueueSetup setup);
        void DeclareQueue(QueueSetup queueSetup);
    }
}