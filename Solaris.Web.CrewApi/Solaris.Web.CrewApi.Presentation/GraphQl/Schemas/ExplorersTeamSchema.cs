using Solaris.Web.CrewApi.Core.GraphQl.Helpers;
using Solaris.Web.CrewApi.Core.GraphQl.Root;
using Solaris.Web.CrewApi.Presentation.GraphQl.Mutations;
using Solaris.Web.CrewApi.Presentation.GraphQl.Queries;

namespace Solaris.Web.CrewApi.Presentation.GraphQl.Schemas
{
    public class ExplorersTeamSchema : ISchemaGroup
    {
        private const string FIELD_NAME = "ExplorersTeams";
        public void SetGroup(RootQuery query)
        {
            query.Field<ExplorersTeamQueries>(
                FIELD_NAME,
                resolve: context => new { });
        }

        public void SetGroup(RootMutation mutation)
        {
            mutation.Field<ExplorersTeamMutations>(
                FIELD_NAME,
                resolve: context => new { });
        }
    }
}