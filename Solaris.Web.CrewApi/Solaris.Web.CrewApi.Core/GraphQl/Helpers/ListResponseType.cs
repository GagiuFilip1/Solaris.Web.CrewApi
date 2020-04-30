using System.Collections.Generic;
using GraphQL.Types;

namespace Solaris.Web.CrewApi.Core.GraphQl.Helpers
{
    public class ListResponseType<T> : ObjectGraphType<object> where T : IGraphType
    {
        protected ListResponseType()
        {
            Field<IntGraphType>().Name("totalCount")
                .Description("A count of the total number of objects in this connection, ignoring pagination.");
            Field<ListGraphType<T>>().Name("items")
                .Description("A list of all of the objects returned in the connection.");
        }
    }

    public class ListResponse<T>
    {
        public long TotalCount { get; set; }
        public IList<T> Items { get; set; }
    }
}