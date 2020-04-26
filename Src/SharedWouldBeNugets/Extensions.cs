using System;
using System.Collections.Generic;
using System.Reflection;

namespace SharedWouldBeNugets
{
    public static class Extensions
    {
        public static List<Type> GetTypesAssignableFrom<T>(this Assembly assembly)
        {
            return assembly.GetTypesAssignableFrom(typeof(T));
        }

        public static List<Type> GetTypesAssignableFrom(this Assembly assembly, Type compareType)
        {
            var ret = new List<Type>();
            foreach (var type in assembly.DefinedTypes)
                if (compareType.IsAssignableFrom(type) && compareType != type)
                    ret.Add(type);
            return ret;
        }
    }
}