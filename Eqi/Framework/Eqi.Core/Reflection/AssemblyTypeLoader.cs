using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Eqi.Core.Reflection
{
    /// <summary>
    /// Default assembly type loader.
    /// </summary>
    public static class AssemblyTypeLoader
    {
        /// <summary>
        /// Get all types from assemblies.
        /// </summary>
        /// <param name="assembly">Assembly instance.</param>
        /// <returns>Type collection.</returns>
        public static IEnumerable<Type> GetTypes(Assembly assembly)
        {
            string message = string.Empty;
            return GetTypes(assembly, out message);
        }

        /// <summary>
        /// Get all types from assemblies.
        /// </summary>
        /// <param name="assembly">Assembly instance.</param>
        /// <param name="message">Process message.</param>
        /// <returns>Type collection.</returns>
        public static IEnumerable<Type> GetTypes(Assembly assembly, out string message)
        {
            return GetTypes(assembly, null, out message);
        }

        /// <summary>
        /// Get filted types from assemblies.
        /// </summary>
        /// <param name="assembly">Assembly instance.</param>
        /// <param name="filter">Filter function.</param>
        /// <returns>Type collection.</returns>
        public static IEnumerable<Type> GetTypes(Assembly assembly, Func<Type, bool> filter)
        {
            string message = string.Empty;
            return GetTypes(assembly, filter, out message);
        }

        /// <summary>
        /// Get filted types from assemblies.
        /// </summary>
        /// <param name="assembly">Assembly instance.</param>
        /// <param name="filter">Filter function.</param>
        /// <param name="message">Process message.</param>
        /// <returns>Type collection.</returns>
        public static IEnumerable<Type> GetTypes(Assembly assembly, Func<Type, bool> filter, out string message)
        {
            IEnumerable<Type> result = Enumerable.Empty<Type>();
            message = string.Empty;

            try
            {
                result = filter != null ? assembly.GetTypes().Where(filter).ToArray() : assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                StringBuilder builder = new StringBuilder();
                if (ex.Types != null && ex.Types.Any())
                {
                    ex.Types.Where(type => type != null).ToList().ForEach(type => builder.AppendFormat("Load type: \"{0}\" fail. ", type.FullName));
                }

                if (ex.Types != null && ex.Types.Any())
                {
                    ex.LoaderExceptions.Where(x => x != null).ToList().ForEach(x => builder.AppendFormat("Load exception: \"{0}\". ", x.Message));
                }

                message = builder.ToString();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return result;
        }
    }
}
