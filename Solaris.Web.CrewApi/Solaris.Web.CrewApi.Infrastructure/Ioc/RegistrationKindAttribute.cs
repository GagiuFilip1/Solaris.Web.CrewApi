using System;

namespace Solaris.Web.CrewApi.Infrastructure.Ioc
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RegistrationKindAttribute : Attribute
    {
        public RegistrationType Type { get; set; }
        public bool AsSelf { get; set; }
    }
}