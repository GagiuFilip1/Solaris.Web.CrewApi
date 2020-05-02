using GraphQL.Types;

namespace Solaris.Web.CrewApi.Core.GraphQl.InputObjects.Robot
{
    public class RobotCreateViewModel : InputObjectGraphType<Models.Entities.Robot>
    {
        public RobotCreateViewModel()
        {
            Field(x => x.Name);
            Field(x => x.CreationDate).Description("The date this robot was created");
            Field(x => x.ProductNumber).Description("The robot Product Number");
            Field(x => x.CurrentStatus).Description("The current robot status (Free, Occupied, Broken)");
            Field(x => x.ExplorersTeamId, type: typeof(NonNullGraphType<IdGraphType>)).Description("The id of the explorers team this Robot is assigned to");
            Field(x => x.CurrentPlanetId, type: typeof(IdGraphType)).Description("The id of the planet this Robot is assigned to");
        }
    }
}