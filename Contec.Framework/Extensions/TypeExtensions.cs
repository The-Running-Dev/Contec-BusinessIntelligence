using System;
using System.Linq;
using System.Reflection;

namespace Contec.Framework.Extensions
{
    public static class TypeExtensions
    {
        public static bool HasCustomAttribute<TAttribute>(this MemberInfo member)
            where TAttribute : Attribute
        {
            return member.GetCustomAttributes(typeof(TAttribute), false).Any();
        }

        public static bool IsNullable(this Type theType)
        {
            return (!theType.IsValueType) || theType.IsNullableOfT();
        }

        public static bool IsNullableOfT(this Type theType)
        {
            return theType.IsGenericType && theType.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static bool IsNullableOf(this Type theType, Type otherType)
        {
            return theType.IsNullableOfT() && theType.GetGenericArguments()[0] == otherType;
        }
    }
}
