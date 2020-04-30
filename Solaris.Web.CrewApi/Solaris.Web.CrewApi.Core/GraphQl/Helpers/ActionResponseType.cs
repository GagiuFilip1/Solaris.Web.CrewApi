using System;
using GraphQL.Types;

namespace Solaris.Web.CrewApi.Core.GraphQl.Helpers
{
    public class ActionResponseType : ObjectGraphType<ActionResponse>
    {
        public ActionResponseType()
        {
            Field(x => x.Success);
            Field(x => x.ObjectId);
        }
    }

    public class ActionResponse
    {
        public ActionResponse(bool success)
        {
            Success = success;
        }

        public ActionResponse(bool success, Guid objectId)
        {
            Success = success;
            ObjectId = objectId;
        }

        public bool Success { get;  }

        public Guid ObjectId { get; }
    }
}