using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Solaris.Web.CrewApi.Core.Models.Helpers;
using Solaris.Web.CrewApi.Infrastructure.Ioc;

namespace Solaris.Web.CrewApi.Infrastructure.Rabbit
{
    [RegistrationKind(Type = RegistrationType.Singleton, AsSelf = true)]
    public class RpcClient
    {
        private readonly AppSettings m_appSettings;
        private ConnectionFactory Factory { get; set; }

        public RpcClient(IOptions<AppSettings> appSettings)
        {
            m_appSettings = appSettings.Value;
            Initialize();
        }

        private void Initialize()
        {
            Factory = new ConnectionFactory
            {
                HostName = m_appSettings.RabbitMq.Host,
                Port = m_appSettings.RabbitMq.Port,
                UserName = m_appSettings.RabbitMq.Username,
                Password = m_appSettings.RabbitMq.Password
            };
        }

        public T PublishRpc<T>(RpcOptions options)
        {
            using var rpcData = new RpcData(Factory, options.Headers);
            rpcData.Channel.BasicPublish(
                "",
                options.TargetQueue,
                rpcData.BasicProperties,
                Encoding.UTF8.GetBytes(options.Message));

            rpcData.Channel.BasicConsume(rpcData.Consumer, rpcData.ReplyQueueName, true);
            var received = rpcData.ResponseQueue.Take();
            
            return JsonConvert.DeserializeObject<T>(received);
        }
    }
}