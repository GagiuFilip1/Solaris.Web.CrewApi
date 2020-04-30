using Solaris.Web.CrewApi.Core.GraphQl.Root;

namespace Solaris.Web.CrewApi.Core.GraphQl.Helpers
{
    public interface ISchemaGroup
    {
        void SetGroup(RootQuery query);
        void SetGroup(RootMutation mutation);
    }
}