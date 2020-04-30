using System.Collections.Generic;
using GraphQL.Types;
using Solaris.Web.CrewApi.Core.GraphQl.Helpers;

namespace Solaris.Web.CrewApi.Core.GraphQl.Root
{
    public class RootQuery : ObjectGraphType
    {
        public RootQuery(IEnumerable<ISchemaGroup> queries)
        {
            Name = "Query";
            Description =
                "On search queries if you want to filter by a specific account always set Filtering.AccountId field in the request body. " +
                "The Account Id in the Header is responsible only for checking if user is authorized to run the query" +
                "If specified, the Account Id in the header has to be equal with the Account Id in the body.";
            foreach (var query in queries)
            {
                query.SetGroup(this);
            }
        }
    }
}