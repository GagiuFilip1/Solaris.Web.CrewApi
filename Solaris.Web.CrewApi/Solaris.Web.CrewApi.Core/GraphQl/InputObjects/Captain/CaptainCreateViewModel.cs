using GraphQL.Types;

namespace Solaris.Web.CrewApi.Core.GraphQl.InputObjects.Captain
{
    public class CaptainCreateViewModel : InputObjectGraphType<Models.Entities.Captain>
    {
        public CaptainCreateViewModel()
        {
            Field(x => x.Name);
            Field(x => x.Gender).Description("The gender of the captain (MALE/ FEMALE)");
            Field(x => x.Age).Description("The captain age, must be between 30 and 65 years");
            Field(x => x.Email).Description("The captain email, used for contacting");
            Field(x => x.ExplorersTeamId, type: typeof(NonNullGraphType<IdGraphType>)).Description("The id of the explorers team this captain is assigned to");
        }
    }
}