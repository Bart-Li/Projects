using System;

namespace Eqi.Core
{
    /// <summary>
    /// Class with this attribute will be auto register as a service,Default lift time is singleton.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DependencyInjectionAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of DependencyInjectionAttribute class.
        /// </summary>
        /// <param name="service">Service.</param>
        public DependencyInjectionAttribute(Type service)
        {
            this.Service = service;
            this.LifeTime = ServiceLifeTime.Singleton;
        }

        /// <summary>
        /// Gets service type.
        /// </summary>
        public Type Service { get; private set; }

        /// <summary>
        /// Gets or sets service lifetime.
        /// </summary>
        public ServiceLifeTime LifeTime { get; set; }

        /// <summary>
        /// Gets or sets priority.
        /// </summary>
        public int Priority { get; set; }
    }
}
