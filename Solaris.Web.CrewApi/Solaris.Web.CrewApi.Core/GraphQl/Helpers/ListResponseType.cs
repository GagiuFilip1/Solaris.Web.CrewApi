using System.Collections.Generic;
using GraphQL.Types;
using Solaris.Web.CrewApi.Core.GraphQl.OutputObjects.Captain;
using Solaris.Web.CrewApi.Core.GraphQl.OutputObjects.CrewMember;
using Solaris.Web.CrewApi.Core.GraphQl.OutputObjects.ExplorersTeam;
using Solaris.Web.CrewApi.Core.GraphQl.OutputObjects.Robot;
using Solaris.Web.CrewApi.Core.GraphQl.OutputObjects.Shuttle;

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
    
    public class ListCaptainsQueryModelType : ListResponseType<CaptainQueryViewModel>
    {
    }
    
    public class ListRobotsQueryModelType : ListResponseType<RobotQueryViewModel>
    {
    }
    
    public class ListShuttlesQueryModelType : ListResponseType<ShuttleQueryViewModel>
    {
    }
    
    public class ListExplorersTeamsQueryModelType : ListResponseType<ExplorersTeamQueryViewModel>
    {
    }
    
    public class ListCrewMembersQueryModelType : ListResponseType<CrewMemberQueryViewModel>
    {
    }
}