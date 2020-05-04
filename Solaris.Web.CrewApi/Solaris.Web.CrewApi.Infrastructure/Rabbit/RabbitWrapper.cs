using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Solaris.Web.CrewApi.Core.Models.Helpers.Commons;
using Solaris.Web.CrewApi.Core.Models.Rabbit.Helpers.Setup;
using Solaris.Web.CrewApi.Core.Models.Rabbit.Interfaces;
using Solaris.Web.CrewApi.Infrastructure.Ioc;

namespace Solaris.Web.CrewApi.Infrastructure.Rabbit
{
    [RegistrationKind(Type = RegistrationType.Scoped, AsSelf = true)]
    public class RabbitWrapper
    {
        private readonly AppSettings m_appSettings;
        private readonly IRabbitHandler m_handler;
        private readonly IEnumerable<IProcessor> m_processors;

        public RabbitWrapper(IOptions<AppSettings> appSettings, IRabbitHandler handler, IEnumerable<IProcessor> processors)
        {
            m_handler = handler;
            m_appSettings = appSettings.Value;
            m_processors = processors;
            SetProcessors();
            InitialiseQueues();
        }

        private void InitialiseQueues()
        {
            m_handler.DeclareRpcQueue(new QueueSetup
            {
                Qos = 10,
                QueueName = m_appSettings.RabbitMqQueues.CrewApiQueue
            });
        }

        private void SetProcessors()
        {
            foreach (var processor in m_processors) m_handler.Processors.TryAdd(processor.Type, processor.ProcessAsync);
        }
    }
}