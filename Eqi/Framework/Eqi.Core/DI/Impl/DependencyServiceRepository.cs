using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Eqi.Core.DI.Impl;
using Eqi.Core.Reflection;

namespace Eqi.Core.DI
{
    public static class DependencyServiceRepository
    {
        /// <summary>
        /// Service definition collection.
        /// </summary>
        public static IServiceDefinitionCollection BuildServiceDefinitionCollection()
        {
            var serviceDefinitionCollection = new DefaultServiceDefinitionCollection();
            var serviceDefinitions = GetServicesDefinition();
            var definitions = serviceDefinitions as IList<IServiceDefinition> ?? serviceDefinitions.ToList();
            if (definitions.Any())
            {
                definitions.ToList().ForEach(serviceDefinition =>
                {
                    if (serviceDefinition.ImplementInstance != null)
                    {
                        serviceDefinitionCollection.RegisterService(serviceDefinition.ServiceType, serviceDefinition.ImplementInstance, serviceDefinition.LifeTime);
                    }
                    else if (serviceDefinition.ImplementFactory != null)
                    {
                        serviceDefinitionCollection.RegisterService(serviceDefinition.ServiceType, serviceDefinition.ImplementFactory, serviceDefinition.LifeTime);
                    }
                    else
                    {
                        serviceDefinitionCollection.RegisterService(serviceDefinition.ServiceType, serviceDefinition.ImplementType, serviceDefinition.LifeTime);
                    }
                });
            }

            return serviceDefinitionCollection;
        }

        /// <summary>
        /// Get service definitions.
        /// </summary>
        /// <returns>Service definitions.</returns>
        private static IEnumerable<IServiceDefinition> GetServicesDefinition()
        {
            var serviceList = new List<IServiceDefinition>();

            IEnumerable<Assembly> assembiles = AssemblyLoader.LoadAll();
            IEnumerable<Type> services = assembiles.SelectMany(assembly => AssemblyTypeLoader.GetTypes(assembly, ContainsAutoSetupServiceAttribute));
            var serviceGroup = services.SelectMany(service => service.GetCustomAttributes<DependencyInjectionAttribute>().Select(attr => new { Attr = attr, Impl = service }))
                                    .GroupBy(define => define.Attr.Service);


            foreach (var service in serviceGroup)
            {
                var highPriorityImpl = service.OrderByDescending(impl => impl.Attr.Priority).First();
                serviceList.Add(new ServiceDefinition(highPriorityImpl.Attr.Service, highPriorityImpl.Impl, highPriorityImpl.Attr.LifeTime));
            }

            return serviceList;
        }

        /// <summary>
        /// Check whether type contains AutoSetupServiceAttribute.
        /// </summary>
        /// <param name="type">Current type.</param>
        /// <returns>True contain, false not contain.</returns>
        private static bool ContainsAutoSetupServiceAttribute(Type type)
        {
            if (!type.IsClass)
            {
                return false;
            }

            if (type.IsAbstract)
            {
                return false;
            }

            return type.GetCustomAttribute<DependencyInjectionAttribute>() != null;
        }
    }
}
