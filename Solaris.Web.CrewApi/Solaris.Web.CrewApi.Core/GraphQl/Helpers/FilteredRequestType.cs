using GraphQL.Types;
using Solaris.Web.CrewApi.Core.Models.Filters.Interfaces;
using Solaris.Web.CrewApi.Core.Models.Interfaces.Commons;

namespace Solaris.Web.CrewApi.Core.GraphQl.Helpers
{
    public class FilteredRequestType<T> : InputObjectGraphType<IFilter<T>> where T : IIdentifier
    {
        public new const string Description = "Filter used to refine the search response";

        public FilteredRequestType()
        {
            Field(x => x.SearchTerm, true).Description($"The search term which will be used to filter the list of {typeof(T).Name}s");
            Field(x => x.Ids, true, typeof(ListGraphType<IdGraphType>)).Description($"A list of ids used to filter {typeof(T).Name}s");
        }
    }
}