using GraphQL.Types;

namespace Solaris.Web.CrewApi.Core.GraphQl.OutputObjects.Captain
{
    public class CaptainQueryViewModel : ObjectGraphType<Models.Entities.Captain>
    {
        public CaptainQueryViewModel()
        {
            Field(x => x.Id, type: typeof(NonNullGraphType<GuidGraphType>));
            Field(x => x.Name);
            Field(x => x.Gender).Description("The gender of the captain (MALE/ FEMALE)");
            Field(x => x.Age).Description("The captain age, must be between 30 and 65 years");
            Field(x => x.Email).Description("The captain email, used for contacting");
            Field(x => x.Type);
            Field(x => x.ExplorersTeamId, type: typeof(NonNullGraphType<GuidGraphType>)).Description("The id of the explorers team this captain is assigned to");
        }
    }
}