using System.Collections.Generic;
using Autofac;

namespace Eqi.Core.DI
{
    /// <summary>
    /// Autofac services registor.
    /// </summary>
    public interface IAutofacServicesRegistor
    {
        /// <summary>
        /// Registor services to container builder.
        /// </summary>
        /// <param name="builder">Container builder.</param>
        /// <param name="services">Services definition.</param>
        void Registor(ContainerBuilder builder, IEnumerable<IServiceDefinition> services);
    }
}
