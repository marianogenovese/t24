//-----------------------------------------------------------------------
// <copyright file="RuntimeConfiguration.cs" company="Integra.Vision.Engine.Host">
//     Copyright (c) Integra.Vision.Engine.Host. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Host.Runtime
{
    using System.Configuration;
    using System.Diagnostics.Contracts;
    using Integra.Vision.Engine.Host.Configuration;
    
    /// <summary>
    /// Use the hosting configuration to expose runtime configuration information
    /// </summary>
    internal sealed class RuntimeConfiguration
    {
        /// <summary>
        /// Current configuration.
        /// </summary>
        private static RuntimeConfiguration configurationInstance;

        /// <summary>
        /// Runtime base path.
        /// </summary>
        private string basePath;

        /// <summary>
        /// Engine app.config file path.
        /// </summary>
        private string engineAppConfigFilePath = null;

        /// <summary>
        /// The hosting configuration.
        /// </summary>
        private HostingConfigurationSection configuration;

        /// <summary>
        /// Gets the current runtime configuration.
        /// </summary>
        public static RuntimeConfiguration Current
        {
            get
            {
                lock (typeof(RuntimeConfiguration))
                {
                    if (configurationInstance == null)
                    {
                        configurationInstance = new RuntimeConfiguration();
                    }
                }

                return configurationInstance;
            }
        }

        /// <summary>
        /// Gets or sets the Runtime base path.
        /// </summary>
        [Pure]
        public string BasePath
        {
            get
            {
                Contract.Ensures(ArgumentOperations.IsValidDirectory(this.basePath));
                return this.basePath;
            }
            
            set
            {
                Contract.Requires(ArgumentOperations.IsValidDirectory(value));
                this.basePath = value;
            }
        }

        /// <summary>
        /// Gets the Engine AppConfig file path.
        /// </summary>
        [Pure]
        public string EngineAppConfigFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(this.engineAppConfigFilePath))
                {
                    this.engineAppConfigFilePath = System.IO.Path.Combine(this.BasePath, Resources.SR.EngineAppConfigFileName);
                }
                
                return this.engineAppConfigFilePath;
            }
        }

        /// <summary>
        /// Gets the engine builder type.
        /// </summary>
        public string EngineBuilderType
        {
            get
            {
                Contract.Ensures(!string.IsNullOrEmpty(this.HostingConfiguration.Bootstrap.EngineBuilderType));
                return this.HostingConfiguration.Bootstrap.EngineBuilderType;
            }
        }

        /// <summary>
        /// Gets the hosting configuration.
        /// </summary>
        internal HostingConfigurationSection HostingConfiguration
        {
            get
            {
                Contract.Ensures(Contract.Result<HostingConfigurationSection>() != null, Diagnostics.Resources.SR.HostingConfigurationNotFound);
                
                if (this.configuration == null)
                {
                    this.configuration = ConfigurationManager.GetSection(Resources.SR.HostingConfigurationSection) as HostingConfigurationSection;
                }

                return this.configuration;
            }
        }
    }
}
