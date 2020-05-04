using GraphQL.Server;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Solaris.Web.CrewApi.Core.Models.Helpers.Commons;
using Solaris.Web.CrewApi.Infrastructure.Data;
using Solaris.Web.CrewApi.Infrastructure.Ioc;

namespace Solaris.Web.CrewApi.Presentation
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private const string CONNECTION_STRING_PATH = "ConnectionStrings:CrewApi";
        private const string MIGRATION_ASSEMBLY = "Solaris.Web.CrewApi.Presentation";
        private const string REPOSITORIES_NAMESPACE = "Solaris.Web.CrewApi.Infrastructure.Repositories.Implementations";
        private const string SERVICES_NAMESPACE = "Solaris.Web.CrewApi.Core.Services.Implementations";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(options => { options.AllowSynchronousIO = true; });
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<IISServerOptions>(options => { options.AllowSynchronousIO = true; });
            services.InjectMySqlDbContext<DataContext>(Configuration[CONNECTION_STRING_PATH], MIGRATION_ASSEMBLY);
            services.InjectForNamespace(REPOSITORIES_NAMESPACE);
            services.InjectForNamespace(SERVICES_NAMESPACE);
            services.InjectRabbitMq();
            services.InjectGraphQl();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseGraphQL<ISchema>();
        }
    }
}