using System;
using System.Collections.Generic;
using System.Reflection;

namespace Eqi.Core.Reflection.Impl
{
    /// <summary>
    /// Default app domain.
    /// </summary>
    [DependencyInjection(typeof(ICurrentAppDomain))]
    public class DefaultCurrentAppDomain : ICurrentAppDomain
    {
        /// <summary>
        /// Current app domain.
        /// </summary>
        private readonly AppDomain _current = AppDomain.CurrentDomain;

        /// <summary>
        /// Gets base directory.
        /// </summary>
        public string BaseDirectory => _current.BaseDirectory;

        /// <summary>
        /// Gets dynamic directory.
        /// </summary>
        public string DynamicDirectory => _current.DynamicDirectory;

        /// <summary>
        /// Get appdomain assemblies.
        /// </summary>
        /// <returns>Assembly collection.</returns>
        public IEnumerable<Assembly> GetDomainAssemblies()
        {
            return this._current.GetAssemblies();
        }

        /// <summary>
        /// Add assembly resolve handler.
        /// </summary>
        /// <param name="handler">Resolve event handler.</param>
        public void AddAssemblyResolve(ResolveEventHandler handler)
        {
            this._current.AssemblyResolve += handler;
        }
    }
}
