using System;

namespace Eqi.Core.DI
{
    public interface IServiceCollection
    {
        bool ContainService(Type serviceType);

        bool ContainService<TService>();

        object GetService(Type serviceType);

        TService Resolve<TService>();
    }
}
