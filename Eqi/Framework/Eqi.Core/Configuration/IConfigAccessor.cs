namespace Eqi.Core.Configuration
{
    public interface IConfigAccessor
    {
        T GetConfigValue<T>(IConfigFileDefinition configFile) where T : class;
    }
}
