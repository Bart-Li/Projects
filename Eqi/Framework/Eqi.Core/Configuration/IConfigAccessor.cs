namespace Eqi.Core.Configuration
{
    public interface IConfigAccessor
    {
        T GetConfigValue<T>(ConfigurationFile configFile) where T : class;
    }
}
