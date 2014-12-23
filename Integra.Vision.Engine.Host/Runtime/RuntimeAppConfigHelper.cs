//-----------------------------------------------------------------------
// <copyright file="RuntimeAppConfigHelper.cs" company="Integra.Vision.Engine.Host">
//     Copyright (c) Integra.Vision.Engine.Host. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Host.Runtime
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Reflection;
    
    /// <summary>
    /// Provides helper functions to change dynamically the app config in runtime.
    /// </summary>
    internal static class RuntimeAppConfigHelper
    {
        /// <summary>
        /// File path of the default app.config file.
        /// </summary>
        private static string defaultAppConfigFile;

        /// <summary>
        /// Gets the path for the default app.config file.
        /// </summary>
        private static string DefaultAppConfigFile
        {
            get
            {
                if (string.IsNullOrEmpty(defaultAppConfigFile))
                {
                    defaultAppConfigFile = AppDomain.CurrentDomain.GetData("APP_CONFIG_FILE").ToString();
                }
                
                return defaultAppConfigFile;
            }
        }

        /// <summary>
        /// Change the runtime configuration in the AppDomain
        /// </summary>
        /// <param name="configurationFilePath">The configuration file.</param>
        public static void ApplyConfiguration(string configurationFilePath)
        {
            RestoreConfiguration();
            Diagnostics.DiagnosticHelper.Logger.Debug(Diagnostics.DiagnosticsEventIds.ConfigurationFileEvent, Resources.SR.AppConfigApply(configurationFilePath));
            if (!ArgumentOperations.IsValidFile(configurationFilePath))
            {
                Diagnostics.DiagnosticHelper.Logger.Warning(Diagnostics.DiagnosticsEventIds.ConfigurationFileEvent, Resources.SR.AppConfigInvalid(configurationFilePath));
                return;
            }
            
            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", configurationFilePath);
            ResetConfigMechanism();
        }
        
        /// <summary>
        /// Restore the configuration to the default configuration file.
        /// </summary>
        public static void RestoreConfiguration()
        {
            Diagnostics.DiagnosticHelper.Logger.Debug(Diagnostics.DiagnosticsEventIds.ConfigurationFileEvent, Resources.SR.AppConfigRestore(DefaultAppConfigFile));
            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", DefaultAppConfigFile);
            ResetConfigMechanism();
        }

        /// <summary>
        /// This hack reset the internal variables in the configuration system.
        /// </summary>
        private static void ResetConfigMechanism()
        {
            typeof(ConfigurationManager).GetField("s_initState", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, 0);
            typeof(ConfigurationManager).GetField("s_configSystem", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, null);
            typeof(ConfigurationManager).Assembly.GetTypes().Where(x => x.FullName == "System.Configuration.ClientConfigPaths").First().GetField("s_current", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, null);
        }
    }
}
