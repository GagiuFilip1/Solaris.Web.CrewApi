using Solaris.Web.CrewApi.Core.GraphQl.Helpers;
using Solaris.Web.CrewApi.Core.GraphQl.Root;
using Solaris.Web.CrewApi.Presentation.GraphQl.Queries;

namespace Solaris.Web.CrewApi.Presentation.GraphQl.Schemas
{
    public class CrewMemberSchema : ISchemaGroup
    {
        private const string FIELD_NAME = "CrewMembers";
        public void SetGroup(RootQuery query)
        {
            query.Field<CrewMemberQueries>(
                FIELD_NAME,
                resolve: context => new { });
        }

        public void SetGroup(RootMutation mutation)
        {
            
        }
    }
}