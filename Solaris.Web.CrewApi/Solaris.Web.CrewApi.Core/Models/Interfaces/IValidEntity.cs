using System.Collections.Generic;

namespace Solaris.Web.CrewApi.Core.Models.Interfaces
{
    public interface IValidEntity
    {
        List<string> Validate();
    }
}