using System;
using Autofac;

namespace Eqi.Core.DI.Impl
{
    [DependencyInjection(typeof(IServiceCollection))]
    public class DefaultServiceCollection: IServiceCollection
    {
        /// <summary>
        /// Main container builder.
        /// </summary>
        private readonly ContainerBuilder mainBuilder = new ContainerBuilder();

        /// <summary>
        /// Main container.
        /// </summary>
        private readonly IContainer mainContainer = null;

        public DefaultServiceCollection(IServiceDefinitionCollection serviceDefinitionCollection)
        {
            new DefaultAutofacServicesRegistor().Registor(this.mainBuilder, serviceDefinitionCollection.ServiceDefinitionCollection);
            this.mainContainer = this.mainBuilder.Build();
            var anotherBuilder = new ContainerBuilder();
            anotherBuilder.RegisterInstance(this).As<IServiceCollection>().SingleInstance();
            anotherBuilder.Update(this.mainContainer.ComponentRegistry);
        }

        /// <summary>
        /// Check whether the service has been registered in service locator.
        /// </summary>
        /// <typeparam name="TService">Type of service.</typeparam>
        /// <returns>True if the service has been registered; otherwise false.</returns>
        public bool ContainService<TService>()
        {
            return this.mainContainer.IsRegistered<TService>();
        }

        /// <summary>
        /// Check whether the service has been registered in service locator.
        /// </summary>
        /// <param name="serviceType">Service type.</param>
        /// <returns>True if the service has been registered; otherwise false.</returns>
        public bool ContainService(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            return this.mainContainer.IsRegistered(serviceType);
        }

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type serviceType.-or- null if there is no service object of type serviceType.</returns>
        public object GetService(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            object result = null;

            if (this.ContainService(serviceType))
            {
                result = this.mainContainer.Resolve(serviceType);
            }

            return result;
        }

        /// <summary>
        /// Get service from service collection.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        public TService Resolve<TService>()
        {
            TService result = default(TService);

            if (this.ContainService<TService>())
            {
                result = this.mainContainer.Resolve<TService>();
            }

            return result;
        }
    }
}
