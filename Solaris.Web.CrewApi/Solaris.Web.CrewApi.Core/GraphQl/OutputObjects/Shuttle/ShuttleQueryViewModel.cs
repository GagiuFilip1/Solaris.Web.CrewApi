using GraphQL.Types;

namespace Solaris.Web.CrewApi.Core.GraphQl.OutputObjects.Shuttle
{
    public class ShuttleQueryViewModel : ObjectGraphType<Models.Entities.Shuttle>
    {
        public ShuttleQueryViewModel()
        {
            Field(x => x.Id, type: typeof(NonNullGraphType<GuidGraphType>));
            Field(x => x.Name);
            Field(x => x.CreationDate).Description("The date this Shuttle was created");
            Field(x => x.ShipNumber).Description("The Ship Number of this shuttle");
            Field(x => x.ExplorersTeamId, type: typeof(NonNullGraphType<GuidGraphType>)).Description("The id of the explorers team this Shuttle is assigned to");
        }
    }
}