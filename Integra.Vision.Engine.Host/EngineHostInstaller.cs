//-----------------------------------------------------------------------
// <copyright file="EngineHostInstaller.cs" company="Integra.Vision.Engine.Host">
//     Copyright (c) Integra.Vision.Engine.Host. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Host
{
    using System;
    using System.ComponentModel;
    using System.ServiceProcess;
    
    /// <summary>
    /// Integra Vision Engine Windows Service Installer
    /// </summary>
    [RunInstaller(true)]
    public sealed class EngineHostInstaller : ServiceHostInstallerBase
    {
        /// <summary>
        /// Gets the service name which the service is installed
        /// </summary>
        protected override string Name
        {
            get
            {
                return Resources.SR.ServiceName;
            }
        }
        
        /// <summary>
        /// Gets the service description which the service is installed
        /// </summary>
        protected override string Description
        {
            get
            {
                return Resources.SR.ServiceDescription;
            }
        }

        /// <summary>
        /// Gets the service display name which the service is installed
        /// </summary>
        protected override string DisplayName
        {
            get
            {
                return Resources.SR.ServiceDisplayName;
            }
        }
        
        /// <summary>
        /// Gets the account under which the service is installed
        /// </summary>
        protected override System.ServiceProcess.ServiceAccount Account
        {
            get
            {
                return ServiceAccount.LocalService;
            }
        }
        
        /// <summary>
        /// Add a startup parameters
        /// </summary>
        /// <param name="savedState">Internal dictionary used in Windows Service install process</param>
        protected override void OnBeforeInstall(System.Collections.IDictionary savedState)
        {
            Context.Parameters["assemblypath"] = Context.Parameters["assemblypath"] + " -p=\"" + ArgumentOperations.BasePathArgument + "\"";
            base.OnBeforeInstall(savedState);
        }
    }
}
