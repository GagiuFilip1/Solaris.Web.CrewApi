using GraphQL.Types;

namespace Solaris.Web.CrewApi.Core.GraphQl.InputObjects.ExplorersTeam
{
    public class ExplorersTeamCreateViewModel : InputObjectGraphType<Models.Entities.ExplorersTeam>
    {
        public ExplorersTeamCreateViewModel()
        {
            Field(x => x.Name);
            Field(x => x.DepartedAt).Description("The date this explorers team started their exploration mission");
        }
    }
}