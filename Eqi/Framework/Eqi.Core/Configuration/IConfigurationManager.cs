namespace Eqi.Core.Configuration
{
    /// <summary>
    /// Configuration manager.
    /// </summary>
    public interface IConfigurationManager
    {
        /// <summary>
        /// Get an instance of the given configuration with attribute or name.
        /// </summary>
        /// <typeparam name="TConfig">Type of configuration.</typeparam>
        /// <returns>The requested configuration.</returns>
        TConfig GetConfiguration<TConfig>() where TConfig : class;


        /// <summary>
        /// Get an instance of the given configuration by file path.
        /// </summary>
        /// <typeparam name="TConfig">Type of configuration.</typeparam>
        /// <param name="filePath">Config file path. File path is support absolute and relative paths. Relastive path is base on appdomain + configuration folder.</param>
        /// <returns>The requested configuration.</returns>
        TConfig GetConfiguration<TConfig>(string filePath) where TConfig : class;

        /// <summary>
        /// Get an instance of the given configuration by key.
        /// </summary>
        /// <typeparam name="TConfig">Type of configuration.</typeparam>
        /// <param name="key">Requested key.</param>
        /// <returns>The requested configuration.</returns>
        TConfig GetConfigurationByKey<TConfig>(string key) where TConfig : class;    
    }
}
