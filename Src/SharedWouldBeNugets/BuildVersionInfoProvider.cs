using System.IO;
using System.Linq;
using System.Reflection;

namespace SharedWouldBeNugets
{
    public static class BuildVersionInfoProvider
    {
        public static string GetBuildVersionInfoString(Assembly assembly)
        {
            var assemblyFullName = assembly.FullName.Split(",").First();
            var versionTime = File.GetLastWriteTime(assembly.Location) +
                              " // " +
                              File.GetCreationTimeUtc(assembly.Location);
            return assemblyFullName + " " + versionTime;
        }
    }
}