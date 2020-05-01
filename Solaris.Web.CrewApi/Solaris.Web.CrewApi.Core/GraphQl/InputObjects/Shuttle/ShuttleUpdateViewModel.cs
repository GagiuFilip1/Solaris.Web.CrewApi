using GraphQL.Types;

namespace Solaris.Web.CrewApi.Core.GraphQl.InputObjects.Shuttle
{
    public class ShuttleUpdateViewModel : InputObjectGraphType<Models.Entities.Shuttle>
    {
        public ShuttleUpdateViewModel()
        {
            Field(x => x.Id, type: typeof(NonNullGraphType<IdGraphType>)).Description("The id of the shuttle to be updated");
            Field(x => x.Name);
            Field(x => x.CreationDate).Description("The date this Shuttle was created");
            Field(x => x.ShipNumber).Description("The Ship Number of this shuttle");
            Field(x => x.ExplorersTeamId, type: typeof(NonNullGraphType<IdGraphType>)).Description("The id of the explorers team this Shuttle is assigned to");
        }
    }
}