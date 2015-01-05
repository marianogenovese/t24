//-----------------------------------------------------------------------
// <copyright file="ServiceHostInstallerBase.cs" company="Integra.Vision.Engine.Host">
//     Copyright (c) Integra.Vision.Engine.Host. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Host
{
    using System.ComponentModel;
    using System.Configuration.Install;
    using System.ServiceProcess;

    /// <summary>
    /// This class contains Windows Service Installer base class 
    /// </summary>
    [RunInstaller(true)]
    public abstract class ServiceHostInstallerBase : Installer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceHostInstallerBase"/> class
        /// </summary>
        public ServiceHostInstallerBase()
        {
            this.Initialize();
        }

        /// <summary>
        /// Gets the service name which the service is installed
        /// </summary>
        protected abstract string Name
        {
            get;
        }

        /// <summary>
        /// Gets the service description which the service is installed
        /// </summary>
        protected abstract string Description
        {
            get;
        }

        /// <summary>
        /// Gets the service display name which the service is installed
        /// </summary>
        protected abstract string DisplayName
        {
            get;
        }
        
        /// <summary>
        /// Gets the account under which the service is installed
        /// </summary>
        protected abstract ServiceAccount Account
        {
            get;
        }

        /// <summary>
        /// Initialize routine
        /// </summary>
        private void Initialize()
        {
            ServiceProcessInstaller processInstaller = new ServiceProcessInstaller
            {
                Account = this.Account
            };
            ServiceInstaller serviceInstaller = new ServiceInstaller
            {
                ServiceName = this.Name,
                Description = this.Description,
                DisplayName = this.DisplayName,
                StartType = ServiceStartMode.Manual
            };
            Installers.AddRange(new Installer[] { processInstaller, serviceInstaller });
        }
    }
}
