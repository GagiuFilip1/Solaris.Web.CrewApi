using GraphQL.Types;

namespace Solaris.Web.CrewApi.Core.GraphQl.OutputObjects.Robot
{
    public class RobotQueryViewModel : ObjectGraphType<Models.Entities.Robot>
    {
        public RobotQueryViewModel()
        {
            Field(x => x.Id, type: typeof(NonNullGraphType<GuidGraphType>));
            Field(x => x.Name);
            Field(x => x.CreationDate).Description("The date this robot was created");
            Field(x => x.ProductNumber).Description("The robot Product Number");
            Field(x => x.CurrentStatus).Description("The current robot status (Free, Occupied, Broken)");
            Field(x => x.Type);
            Field(x => x.ExplorersTeamId, type: typeof(NonNullGraphType<GuidGraphType>)).Description("The id of the explorers team this Robot is assigned to");
        }
    }
}