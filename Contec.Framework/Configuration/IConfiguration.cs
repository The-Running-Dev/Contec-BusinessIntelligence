using System.Collections.Generic;

namespace Contec.Framework.Configuration
{
    /// <summary>
    /// Defines the the properties and methods that can be used to pass around an application's
    /// configuration settings.
    /// </summary>
    public interface IConfiguration
    {
        //event ConfigurationEventHandler OnResetComplete;
        event ConfigurationEventHandler OnReloadComplete;

        /// <summary>
        /// Returns a collection of all of the configuration groups defined in the configuration
        /// object.
        /// </summary>
        IDictionary<string, IConfiguration> Groups { get; }

        /// <summary>
        /// Returns the collection of all keys defined in the configuration object.
        /// </summary>
        IEnumerable<string> Keys { get; }

        /// <summary>
        /// Retrieves the requested configuration setting from some configuration source.
        /// </summary>
        /// <param name="settingName">The name of the setting to retrieve a value for.</param>
        /// <returns>The string value for the requested setting.</returns>
        string this[string settingName] { get; }

        /// <summary>
        /// Get the requested setting as a string value.
        /// </summary>
        /// <param name="settingName">The name of the setting to retrieve a value for.</param>
        /// <param name="defaultValue">The default value to return if the requested setting is
        /// not  defined.</param>
        /// <returns>The value for the requested setting or the given default value if it
        /// is not defined.</returns>
        string GetSetting(string settingName, string defaultValue = "");

        /// <summary>
        /// Get the requested setting as a string value.
        /// </summary>
        /// <param name="groupName">The name of the group with the setting to retrieve a value for.</param>
        /// <param name="settingName">The name of the setting to retrieve a value for.</param>
        /// <param name="defaultValue">The default value to return if the requested setting is
        /// not  defined.</param>
        /// <returns>The value for the requested setting or the given default value if it
        /// is not defined.</returns>
        string GetGroupSetting(string groupName, string settingName, string defaultValue = "");

        /// <summary>
        /// Get the requested setting as a T value.
        /// </summary>
        /// <param name="settingName">The name of the setting to retrieve a value for.</param>
        /// <param name="defaultValue">The default value to return if the requested setting is
        /// not  defined.</param>
        /// <returns>The T value for the requested setting or the given default value if it
        /// is not defined.</returns>
        T GetSetting<T>(string settingName, T defaultValue);

        /// <summary>
        /// Get the requested setting as a T value.
        /// </summary>
        /// <param name="groupName">The name of the group with the setting to retrieve a value for.</param>
        /// <param name="settingName">The name of the setting to retrieve a value for.</param>
        /// <param name="defaultValue">The default value to return if the requested setting is
        /// not  defined.</param>
        /// <returns>The T value for the requested setting or the given default value if it
        /// is not defined.</returns>
        T GetGroupSetting<T>(string groupName, string settingName, T defaultValue);

        void Reset();

        void Reload();
    }
}
