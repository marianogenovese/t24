//-----------------------------------------------------------------------
// <copyright file="EngineConfiguration.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.Configuration;
    using System.Diagnostics.Contracts;
    using Integra.Vision.Engine.Configuration;

    /// <summary>
    /// Use the hosting configuration to expose runtime configuration information
    /// </summary>
    internal sealed class EngineConfiguration
    {
        /// <summary>
        /// Current configuration.
        /// </summary>
        private static EngineConfiguration configurationInstance;

        /// <summary>
        /// The hosting configuration.
        /// </summary>
        private EngineConfigurationSection configuration;

        /// <summary>
        /// Gets the current runtime configuration.
        /// </summary>
        public static EngineConfiguration Current
        {
            get
            {
                lock (typeof(EngineConfiguration))
                {
                    if (configurationInstance == null)
                    {
                        configurationInstance = new EngineConfiguration();
                    }
                }

                return configurationInstance;
            }
        }

        /// <summary>
        /// Gets the listener configuration.
        /// </summary>
        internal ListenerConfigurationElement Listener
        {
            get
            {
                return this.Configuration.Listener;
            }
        }

        /// <summary>
        /// Gets the listener configuration.
        /// </summary>
        internal EndpointConfigurationElement DefaultEndpoint
        {
            get
            {
                return this.Configuration.Listener.Endpoints["default"];
            }
        }

        /// <summary>
        /// Gets the buffer management configuration.
        /// </summary>
        internal BufferManagmentConfigurationElement BufferManagement
        {
            get
            {
                return this.Configuration.BufferManagement;
            }
        }

        /// <summary>
        /// Gets the database configuration.
        /// </summary>
        internal DatabaseConfigurationElement Database
        {
            get
            {
                return this.Configuration.Database;
            }
        }

        /// <summary>
        /// Gets the operation configuration.
        /// </summary>
        internal OperationConfigurationElement Operation
        {
            get
            {
                return this.Configuration.Operation;
            }
        }

        /// <summary>
        /// Gets the hosting configuration.
        /// </summary>
        private EngineConfigurationSection Configuration
        {
            get
            {
                Contract.Ensures(Contract.Result<EngineConfigurationSection>() != null, Resources.SR.EngineConfigurationNotFound);

                if (this.configuration == null)
                {
                    this.configuration = ConfigurationManager.GetSection(Resources.SR.EngineConfigurationSection) as EngineConfigurationSection;
                }

                return this.configuration;
            }
        }
    }
}
