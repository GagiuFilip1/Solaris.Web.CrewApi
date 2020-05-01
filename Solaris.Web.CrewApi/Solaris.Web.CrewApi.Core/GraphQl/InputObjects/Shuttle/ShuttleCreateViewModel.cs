using GraphQL.Types;

namespace Solaris.Web.CrewApi.Core.GraphQl.InputObjects.Shuttle
{
    public class ShuttleCreateViewModel : InputObjectGraphType<Models.Entities.Shuttle>
    {
        public ShuttleCreateViewModel()
        {
            Field(x => x.Name);
            Field(x => x.CreationDate).Description("The date this Shuttle was created");
            Field(x => x.ShipNumber).Description("The Ship Number of this shuttle");
            Field(x => x.ExplorersTeamId, type: typeof(NonNullGraphType<IdGraphType>)).Description("The id of the explorers team this Shuttle is assigned to");
        }
    }
}