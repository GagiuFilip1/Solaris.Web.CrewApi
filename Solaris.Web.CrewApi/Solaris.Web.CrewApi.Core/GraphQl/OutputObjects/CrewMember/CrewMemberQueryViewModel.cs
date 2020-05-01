using GraphQL.Types;

namespace Solaris.Web.CrewApi.Core.GraphQl.OutputObjects.CrewMember
{
    public class CrewMemberQueryViewModel : ObjectGraphType<Models.Entities.CrewMember>
    {
        public CrewMemberQueryViewModel()
        {
            Field(x => x.Id, type: typeof(NonNullGraphType<GuidGraphType>));
            Field(x => x.Name);
            Field(x => x.Type);
            Field(x => x.ExplorersTeamId, type: typeof(NonNullGraphType<GuidGraphType>)).Description("The id of the explorers team this captain is assigned to");
        }
    }
}