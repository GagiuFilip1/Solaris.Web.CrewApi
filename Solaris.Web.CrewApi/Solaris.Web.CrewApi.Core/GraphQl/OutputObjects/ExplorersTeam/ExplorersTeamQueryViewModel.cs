using GraphQL.Types;

namespace Solaris.Web.CrewApi.Core.GraphQl.OutputObjects.ExplorersTeam
{
    public class ExplorersTeamQueryViewModel : ObjectGraphType<Models.Entities.ExplorersTeam>
    {
        public ExplorersTeamQueryViewModel()
        {
            Field(x => x.Id, type: typeof(NonNullGraphType<GuidGraphType>));
            Field(x => x.Name);
            Field(x => x.DepartedAt).Description("The date this explorers team started their exploration mission");
        }
    }
}