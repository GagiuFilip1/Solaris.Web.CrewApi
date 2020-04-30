using System.Collections.Generic;
using GraphQL.Types;
using Solaris.Web.CrewApi.Core.GraphQl.Helpers;

namespace Solaris.Web.CrewApi.Core.GraphQl.Root
{
    public class RootMutation : ObjectGraphType
    {
        public RootMutation(IEnumerable<ISchemaGroup> mutations)
        {
            Name = "Mutation";
            Description =
                "On Create mutations always set the Account Id in the body of the request. " +
                "The Account Id in the Header is responsible only for checking if user is authorized to run the mutation" +
                "If specified, the Account Id in the header has to be equal with the Account Id in the body.";
            foreach (var mutation in mutations)
            {
                mutation.SetGroup(this);
            }
        }
    }
}