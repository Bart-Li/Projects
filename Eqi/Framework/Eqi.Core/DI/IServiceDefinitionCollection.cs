using System;
using System.Collections.Generic;

namespace Eqi.Core.DI
{
    /// <summary>
    /// Service defination collection.
    /// </summary>
    public interface IServiceDefinitionCollection
    {
        /// <summary>
        /// Gets service definition collection.
        /// </summary>
        IEnumerable<IServiceDefinition> ServiceDefinitionCollection { get; }

        /// <summary>
        /// Add or update service collection.
        /// </summary>
        /// <param name="serviceType">Service type.</param>
        /// <param name="value">Service definition.</param>
        /// <param name="updateValueFactory">Update value factory.</param>
        /// <returns>Service definition.</returns>
        IServiceDefinition AddOrUpdateServiceDefinition(Type serviceType, IServiceDefinition value, Func<Type, IServiceDefinition, IServiceDefinition> updateValueFactory);

        /// <summary>
        /// Add or update service collection.
        /// </summary>
        /// <param name="serviceType">Service type.</param>
        /// <param name="addValueFactory">Add value factory.</param>
        /// <param name="updateValueFactory">Update value factory.</param>
        /// <returns>Service definition.</returns>
        IServiceDefinition AddOrUpdateServiceDefinition(Type serviceType, Func<Type, IServiceDefinition> addValueFactory, Func<Type, IServiceDefinition, IServiceDefinition> updateValueFactory);
    }
}
