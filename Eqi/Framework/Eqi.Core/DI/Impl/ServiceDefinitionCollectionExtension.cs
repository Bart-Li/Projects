using System;

namespace Eqi.Core.DI.Impl
{
    public static class ServiceDefinitionCollectionExtension
    {
        /// <summary>
        /// Register service with service type and implement type.
        /// </summary>
        /// <typeparam name="TServiceType">Service type.</typeparam>
        /// <typeparam name="TImplementType">Implement type.</typeparam>
        /// <param name="services">Service definition collection.</param>
        /// <param name="lifeTime">Service life time.</param>
        public static IServiceDefinitionCollection RegisterService<TServiceType, TImplementType>(this IServiceDefinitionCollection services, ServiceLifeTime lifeTime)
        {
            return services.RegisterService(typeof(TServiceType), typeof(TImplementType), lifeTime);
        }

        /// <summary>
        /// Register service with service type and implement type.
        /// </summary>
        /// <param name="services">Service definition collection.</param>
        /// <param name="serviceType">Service type.</param>
        /// <param name="implementType">Implement type.</param>
        /// <param name="lifeTime">Service life time.</param>
        public static IServiceDefinitionCollection RegisterService(this IServiceDefinitionCollection services, Type serviceType, Type implementType, ServiceLifeTime lifeTime)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("Service type is null.");
            }

            services.AddOrUpdateServiceDefinition(serviceType,
                CreateServiceDefine(serviceType, null, implementType, null, lifeTime),
                (type, old) => CreateServiceDefine(serviceType, null, implementType, null, lifeTime));

            return services;
        }

        /// <summary>
        /// Register service with service type and implement instance.
        /// </summary>
        /// <typeparam name="TServiceType">Service type.</typeparam>
        /// <param name="services">Service definition collection.</param>
        /// <param name="instance">Implement instance.</param>
        public static IServiceDefinitionCollection RegisterService<TServiceType>(this IServiceDefinitionCollection services, object instance)
        {
            return services.RegisterService<TServiceType>(instance, ServiceLifeTime.Scope);
        }

        /// <summary>
        /// Register service with service type and implement instance.
        /// </summary>
        /// <typeparam name="TServiceType">Service type.</typeparam>
        /// <param name="services">Service definition collection.</param>
        /// <param name="instance">Implement instance.</param>
        public static IServiceDefinitionCollection RegisterService<TServiceType>(this IServiceDefinitionCollection services, object instance, ServiceLifeTime lifeTime)
        {
            return services.RegisterService(typeof(TServiceType), instance, lifeTime);
        }

        /// <summary>
        /// Register service with service type and implement instance.
        /// </summary>
        /// <param name="services">Service definition collection.</param>
        /// <param name="serviceType">Service type.</param>
        /// <param name="instance">Implement instance.</param>
        public static IServiceDefinitionCollection RegisterService(this IServiceDefinitionCollection services, Type serviceType, object instance)
        {
            return services.RegisterService(serviceType, instance, ServiceLifeTime.Scope);
        }

        /// <summary>
        /// Register service with service type and implement instance.
        /// </summary>
        /// <param name="services">Service definition collection.</param>
        /// <param name="serviceType">Service type.</param>
        /// <param name="instance">Implement instance.</param>
        /// <param name="lifeTime">Service life time.</param>
        public static IServiceDefinitionCollection RegisterService(this IServiceDefinitionCollection services, Type serviceType, object instance, ServiceLifeTime lifeTime)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("Service type is null.");
            }

            services.AddOrUpdateServiceDefinition(serviceType,
                CreateServiceDefine(serviceType, instance, null, null, lifeTime),
                (type, old) => CreateServiceDefine(serviceType, instance, null, null, lifeTime));

            return services;
        }

        /// <summary>
        /// Register service with service type and implement factory. 
        /// </summary>
        /// <typeparam name="TServiceType">Service type.</typeparam>
        /// <param name="services">Service definition collection.</param>
        /// <param name="factory">Implement factory.</param>
        /// <param name="lifeTime">Service life time.</param>
        public static IServiceDefinitionCollection RegisterService<TServiceType>(this IServiceDefinitionCollection services, Func<object> factory, ServiceLifeTime lifeTime)
        {
            return services.RegisterService(typeof(TServiceType), factory, lifeTime);
        }

        /// <summary>
        /// Register service with service type and implement factory. 
        /// </summary>
        /// <param name="services">Service definition collection.</param>
        /// <param name="serviceType">Service type.</param>
        /// <param name="factory">Implement factory.</param>
        /// <param name="lifeTime">Service life time.</param>
        public static IServiceDefinitionCollection RegisterService(this IServiceDefinitionCollection services, Type serviceType, Func<object> factory, ServiceLifeTime lifeTime)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("Service type is null.");
            }

            services.AddOrUpdateServiceDefinition(serviceType,
                CreateServiceDefine(serviceType, null, null, factory, lifeTime),
                (type, old) => CreateServiceDefine(serviceType, null, null, factory, lifeTime));

            return services;
        }

        /// <summary>
        /// Create service define.
        /// </summary>
        /// <param name="serviceType">Service type.</param>
        /// <param name="implementInstance">Implemetn instance.</param>
        /// <param name="implementType">Implement type.</param>
        /// <param name="implementFactory">Implement factory.</param>
        /// <param name="lifeTime">Service life time.</param>
        /// <returns>Service defination.</returns>
        private static IServiceDefinition CreateServiceDefine(Type serviceType, object implementInstance, Type implementType, Func<object> implementFactory, ServiceLifeTime lifeTime)
        {
            if (implementInstance == null && implementType == null && implementFactory == null)
            {
                throw new ArgumentNullException("Implement and factory is null.");
            }

            if (implementInstance != null)
            {
                return new ServiceDefinition(serviceType, implementInstance);
            }
            else if (implementFactory != null)
            {
                return new ServiceDefinition(serviceType, implementFactory, lifeTime);
            }
            else
            {
                return new ServiceDefinition(serviceType, implementType, lifeTime);
            }
        }
    }
}
