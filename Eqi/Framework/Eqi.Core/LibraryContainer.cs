using System;
using Eqi.Core.DI;
using Eqi.Core.DI.Impl;

namespace Eqi.Core
{
    /// <summary>
    /// Entry point for the container infrastructure for all Library.
    /// </summary>
    public class LibraryContainer
    {
        /// <summary>
        /// The framework service collection.
        /// </summary>
        private static IServiceCollection servicesCollection;

        /// <summary>
        /// Initializes a new instance of the LibraryContainer class.
        /// </summary>
        static LibraryContainer()
        {
            RegisterService();
        }

        /// <summary>
        /// Gets or sets the current container used to resolve Entlib objects (for use by the various static factories).
        /// </summary>
        public static IServiceCollection Current
        {
            get { return servicesCollection; }
            set { servicesCollection = value; }
        }

        /// <summary>
        /// Add auto setup service.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <returns>Service collection.</returns>
        public static void RegisterService()
        {
            IServiceDefinitionCollection serviceDefinitionCollection = DependencyServiceRepository.BuildServiceDefinitionCollection();
            servicesCollection = new DefaultServiceCollection(serviceDefinitionCollection);
        }

        /// <summary>
        /// Get service from collection.
        /// </summary>
        /// <param name="serviceType">Service type.</param>
        /// <returns>Service instance.</returns>
        public static object GetService(Type serviceType)
        {
            object result = null;
            if (LibraryContainer.Current != null)
            {
                result = LibraryContainer.Current.GetService(serviceType);
            }

            return result;
        }

        /// <summary>
        /// Get service from collection.
        /// </summary>
        /// <typeparam name="TServiceType">Service type.</typeparam>
        /// <returns>Service instance.</returns>
        public static TServiceType Get<TServiceType>() where TServiceType : class
        {
            TServiceType result = default(TServiceType);

            if (LibraryContainer.Current != null)
            {
                result = LibraryContainer.Current.Resolve<TServiceType>();
            }

            return result;
        }
    }
}
