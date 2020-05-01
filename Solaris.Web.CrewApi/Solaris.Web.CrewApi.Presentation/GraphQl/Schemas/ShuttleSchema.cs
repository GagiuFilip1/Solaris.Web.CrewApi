using Solaris.Web.CrewApi.Core.GraphQl.Helpers;
using Solaris.Web.CrewApi.Core.GraphQl.Root;
using Solaris.Web.CrewApi.Presentation.GraphQl.Mutations;
using Solaris.Web.CrewApi.Presentation.GraphQl.Queries;

namespace Solaris.Web.CrewApi.Presentation.GraphQl.Schemas
{
    public class ShuttleSchema : ISchemaGroup
    {
        private const string FIELD_NAME = "Shuttles";
        public void SetGroup(RootQuery query)
        {
            query.Field<ShuttleQueries>(
                FIELD_NAME,
                resolve: context => new { });
        }

        public void SetGroup(RootMutation mutation)
        {
            mutation.Field<ShuttleMutations>(
                FIELD_NAME,
                resolve: context => new { });
        }
    }
}