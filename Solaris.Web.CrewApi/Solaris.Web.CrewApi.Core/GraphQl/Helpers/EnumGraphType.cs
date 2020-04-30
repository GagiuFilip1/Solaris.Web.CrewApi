using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using GraphQL.Types;
using GraphQL.Utilities;

namespace Solaris.Web.CrewApi.Core.GraphQl.Helpers
{
    public sealed class EnumerationGraphType<TEnum> : EnumerationGraphType
    {
        public EnumerationGraphType()
        {
            var enumType = typeof(TEnum);
            Name ??= StringUtils.ToPascalCase(enumType.Name);

            foreach (var name in Enum.GetNames(enumType))
            {
                AddValue(
                    ChangeEnumCase(enumType
                        .GetMember(name, BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public).First().Name),
                    enumType.GetFields().FirstOrDefault(x => x.Name == name)?.GetCustomAttribute<DescriptionAttribute>()?.Description,
                    Enum.Parse(enumType, name), null);
            }
        }

        private static string ChangeEnumCase(string val)
        {
            return StringUtils.ToConstantCase(val);
        }
    }
}