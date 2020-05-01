using GraphQL.Types;

namespace Solaris.Web.CrewApi.Core.GraphQl.InputObjects.ExplorersTeam
{
    public class ExplorersTeamUpdateViewModel : InputObjectGraphType<Models.Entities.ExplorersTeam>
    {
        public ExplorersTeamUpdateViewModel()
        {
            Field(x => x.Id, type: typeof(NonNullGraphType<IdGraphType>)).Description("The Explorers Team Id to be updated");
            Field(x => x.Name);
            Field(x => x.DepartedAt).Description("The date this explorers team started their exploration mission");
        }
    }
}