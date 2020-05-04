using System.Collections.Generic;

namespace Solaris.Web.CrewApi.Core.Models.Interfaces.Commons
{
    public interface IValidEntity
    {
        List<string> Validate();
    }
}