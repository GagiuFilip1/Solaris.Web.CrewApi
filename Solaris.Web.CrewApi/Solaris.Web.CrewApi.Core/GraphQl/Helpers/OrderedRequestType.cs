using GraphQL.Types;
using Solaris.Web.CrewApi.Core.Models.Helpers.Commons;

namespace Solaris.Web.CrewApi.Core.GraphQl.Helpers
{
    public class OrderedRequestType : InputObjectGraphType<Ordering>
    {
        public new static readonly string Description = "Ordering Setting";

        public OrderedRequestType()
        {
            Field(x => x.OrderBy, true).Description("Name of the Property to sort by");
            Field(x => x.OrderDirection, true).Description("Sort direction. Default Ascending");
        }
    }
}