using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SharedWouldBeNugets
{
    public static class BuildVersionInfoProvider
    {
        public static string GetBuildVersionInfoString(Assembly assembly)
        {
            var assemblyFullName = assembly.FullName.Split(",").First();
            var versionTime = File.GetLastWriteTime(assembly.Location);
            return assemblyFullName + " " +versionTime;
        }
    }
}
