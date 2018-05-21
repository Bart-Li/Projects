using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Eqi.Core.DI.Impl
{
    /// <summary>
    /// Service definition collection.
    /// </summary>
    public class DefaultServiceDefinitionCollection : ConcurrentDictionary<Type, IServiceDefinition>, IServiceDefinitionCollection
    {
        /// <summary>
        /// Gets service definition collection.
        /// </summary>
        public IEnumerable<IServiceDefinition> ServiceDefinitionCollection => this.Values.ToList();

        /// <summary>
        /// Add or update service collection.
        /// </summary>
        /// <param name="serviceType">Service type.</param>
        /// <param name="value">Service definition.</param>
        /// <param name="updateValueFactory">Update value factory.</param>
        /// <returns>Service definition.</returns>
        public IServiceDefinition AddOrUpdateServiceDefinition(Type serviceType, IServiceDefinition value, Func<Type, IServiceDefinition, IServiceDefinition> updateValueFactory)
        {
            return this.AddOrUpdate(serviceType, value, updateValueFactory);
        }

        /// <summary>
        /// Add or update service collection.
        /// </summary>
        /// <param name="serviceType">Service type.</param>
        /// <param name="addValueFactory">Add value factory.</param>
        /// <param name="updateValueFactory">Update value factory.</param>
        /// <returns>Service definition.</returns>
        public IServiceDefinition AddOrUpdateServiceDefinition(Type serviceType, Func<Type, IServiceDefinition> addValueFactory, Func<Type, IServiceDefinition, IServiceDefinition> updateValueFactory)
        {
            return this.AddOrUpdate(serviceType, addValueFactory, updateValueFactory);
        }
    }
}
