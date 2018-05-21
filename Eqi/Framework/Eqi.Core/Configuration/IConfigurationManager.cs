namespace Eqi.Core.Configuration
{
    public interface IConfigurationManager
    {
        /// <summary>
        /// Get an instance of the given configuration with bizunit.
        /// </summary>
        /// <typeparam name="TConfig">Type of configuration.</typeparam>
        /// <returns>The requested configuration.</returns>
        TConfig GetConfiguration<TConfig>() where TConfig : class;


        /// <summary>
        /// Get an instance of the given configuration with bizunit.
        /// </summary>
        /// <typeparam name="TConfig">Type of configuration.</typeparam>
        /// <param name="filePath">Config file path. File path is support absolute and relative paths. Relastive path is base on appdomain + configuration folder.</param>
        /// <returns>The requested configuration.</returns>
        TConfig GetConfiguration<TConfig>(string filePath) where TConfig : class;

        /// <summary>
        /// Get an instance of the given configuration by key with bizunit.
        /// </summary>
        /// <typeparam name="TConfig">Type of configuration.</typeparam>
        /// <param name="key">Requested key.</param>
        /// <returns>The requested configuration.</returns>
        TConfig GetConfigurationByKey<TConfig>(string key) where TConfig : class;    
    }
}
