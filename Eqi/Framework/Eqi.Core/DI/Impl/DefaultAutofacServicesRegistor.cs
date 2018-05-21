using System.Collections.Generic;
using Autofac;

namespace Eqi.Core.DI.Impl
{
    /// <summary>
    /// Autofac services registor.
    /// </summary>
    [DependencyInjection(typeof(IAutofacServicesRegistor))]
    public class DefaultAutofacServicesRegistor : IAutofacServicesRegistor
    {
        /// <summary>
        /// Registor services to container builder.
        /// </summary>
        /// <param name="builder">Container builder.</param>
        /// <param name="services">Services definition.</param>
        public void Registor(ContainerBuilder builder, IEnumerable<IServiceDefinition> services)
        {
            if (builder != null && !services.IsNullOrEmpty())
            {
                services.ForEach(service => this.RegisterServiceToBuilder(builder, service));
            }
        }

        /// <summary>
        /// Register service to builder.
        /// </summary>
        /// <param name="builder">Container builder.</param>
        /// <param name="service">Service definition.</param>
        private void RegisterServiceToBuilder(ContainerBuilder builder, IServiceDefinition service)
        {
            if (service.ImplementFactory != null)
            {
                this.RegisterServiceWithFactoryToBuilder(builder, service);
            }
            else if (service.ImplementInstance != null)
            {
                this.RegisterServiceWithInstanceToBuilder(builder, service);
            }
            else
            {
                this.RegisterServiceWithImplementToBuilder(builder, service);
            }
        }

        /// <summary>
        /// Register service with service implement type to builder.
        /// </summary>
        /// <param name="builder">Container builder.</param>
        /// <param name="service">Service definition.</param>
        private void RegisterServiceWithInstanceToBuilder(ContainerBuilder builder, IServiceDefinition service)
        {
            var step1 = builder.RegisterInstance(service.ImplementInstance);
            var step2 = step1.As(service.ServiceType);

            switch (service.LifeTime)
            {
                case ServiceLifeTime.Singleton:
                    step2.SingleInstance();
                    break;
                case ServiceLifeTime.Transient:
                    step2.InstancePerDependency();
                    break;
                case ServiceLifeTime.Scope:
                default:
                    step2.InstancePerLifetimeScope();
                    break;
            }
        }

        /// <summary>
        /// Register service with service implement type to builder.
        /// </summary>
        /// <param name="builder">Container builder.</param>
        /// <param name="service">Service definition.</param>
        private void RegisterServiceWithImplementToBuilder(ContainerBuilder builder, IServiceDefinition service)
        {
            var step1 = builder.RegisterType(service.ImplementType);
            var step2 = step1.As(service.ServiceType);

            switch (service.LifeTime)
            {
                case ServiceLifeTime.Singleton:
                    step2.SingleInstance();
                    break;
                case ServiceLifeTime.Transient:
                    step2.InstancePerDependency();
                    break;
                case ServiceLifeTime.Scope:
                default:
                    step2.InstancePerLifetimeScope();
                    break;
            }
        }

        /// <summary>
        /// Register service with service factory to builder.
        /// </summary>
        /// <param name="builder">Container builder.</param>
        /// <param name="service">Service definition.</param>
        private void RegisterServiceWithFactoryToBuilder(ContainerBuilder builder, IServiceDefinition service)
        {
            var step1 = builder.Register(c => service.ImplementFactory());
            var step2 = step1.As(service.ServiceType);

            switch (service.LifeTime)
            {
                case ServiceLifeTime.Singleton:
                    step2.SingleInstance();
                    break;
                case ServiceLifeTime.Transient:
                    step2.InstancePerDependency();
                    break;
                case ServiceLifeTime.Scope:
                default:
                    step2.InstancePerLifetimeScope();
                    break;
            }
        }
    }
}
