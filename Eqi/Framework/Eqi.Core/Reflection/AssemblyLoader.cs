using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Eqi.Core.Reflection
{
    /// <summary>
    /// Assembly loader.
    /// </summary>
    public static class AssemblyLoader
    {
        private static readonly ConcurrentDictionary<string, Assembly> DicAssemblies = new ConcurrentDictionary<string, Assembly>();

        /// <summary>
        /// Initializes the AssemblyLoader class.
        /// </summary>
        static AssemblyLoader()
        {
            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.GetAssemblies().ToList().ForEach(TryAdd);
            Initialize(new DirectoryInfo(currentDomain.BaseDirectory));
        }

        /// <summary>
        /// Load the specified assemblyName.
        /// </summary>
        /// <returns>The load.</returns>
        /// <param name="assemblyName">Assembly name.</param>
        public static Assembly Load(AssemblyName assemblyName)
        {
            Assembly result;
            if (DicAssemblies.TryGetValue(assemblyName.Name, out result))
            {
                return result;
            }

            return default(Assembly);
        }

        /// <summary>
        /// Loads all.
        /// </summary>
        /// <returns>The all.</returns>
        public static IEnumerable<Assembly> LoadAll()
        {
            return DicAssemblies.Values.ToArray();
        }

        /// <summary>
        /// Initialize the specified directoryInfo.
        /// </summary>
        /// <returns>The initialize.</returns>
        /// <param name="directoryInfo">Directory info.</param>
        private static void Initialize(DirectoryInfo directoryInfo)
        {
            var assembliesDll = directoryInfo.GetFiles("*.dll");
            if (assembliesDll.Any())
            {
                assembliesDll.ToList().ForEach(Initialize);
            }
        }

        /// <summary>
        /// Initialize the specified fileInfo.
        /// </summary>
        /// <returns>The initialize.</returns>
        /// <param name="fileInfo">File info.</param>
        private static void Initialize(FileInfo fileInfo)
        {
            TryAdd(Assembly.LoadFrom(fileInfo.FullName));
        }

        /// <summary>
        /// Tries the add.
        /// </summary>
        /// <param name="assembly">Assembly.</param>
        private static void TryAdd(Assembly assembly)
        {
            DicAssemblies.TryAdd(assembly.FullName, assembly);
        }
    }
}
