//-----------------------------------------------------------------------
// <copyright file="RuntimeEngine.cs" company="Integra.Vision.Engine.Host">
//     Copyright (c) Integra.Vision.Engine.Host. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Host.Runtime
{
    using System;
    using System.Configuration;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Integra.Vision.Dependency;
    using Integra.Vision.Diagnostics;
    using Integra.Vision.Engine.Host.Configuration;
    
    /// <summary>
    /// This class implements the construction and execution in runtime of the engine modules.
    /// </summary>
    internal sealed class RuntimeEngine : IDisposable
    {
        /// <summary>
        /// Runtime dependency resolver.
        /// </summary>
        private IDependencyResolver dependencyResolver;

        /// <summary>
        /// The modules used in the engine.
        /// </summary>
        private RuntimeModule[] modules;

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeEngine"/> class.
        /// </summary>
        /// <param name="basePath">The path for type discover.</param>
        public RuntimeEngine(string basePath)
        {
            Contract.Requires(!string.IsNullOrEmpty(basePath));
            RuntimeConfiguration.Current.BasePath = basePath;
            this.dependencyResolver = new DefaultDependencyResolver();
        }
        
        /// <summary>
        /// Gets the runtime engine modules.
        /// </summary>
        private RuntimeModule[] Modules
        {
            get
            {
                if (this.modules == null)
                {
                    RuntimeModulesBuilder builder = new RuntimeModulesBuilder(this.dependencyResolver);
                    this.modules = builder.Build();
                }

                return this.modules;
            }
        }

        /// <summary>
        /// Start the engine modules.
        /// </summary>
        public void Start()
        {
            Diagnostics.DiagnosticHelper.Logger.Debug(Diagnostics.DiagnosticsEventIds.RuntimeStarted, Diagnostics.Resources.SR.RuntimeStarting);
            this.StartCore();
            Diagnostics.DiagnosticHelper.Logger.Debug(Diagnostics.DiagnosticsEventIds.RuntimeStarted, Diagnostics.Resources.SR.RuntimeStarted);
        }

        /// <summary>
        /// Stops the engine modules.
        /// </summary>
        public void Stop()
        {
            Diagnostics.DiagnosticHelper.Logger.Debug(Diagnostics.DiagnosticsEventIds.ModuleStartEvent, "****** STOPPING MODULES ******");
            
            // Se invierte el orden de los modulos para detenerlos
            foreach (RuntimeModule module in this.Modules.Reverse())
            {
                module.Stop();
            }
            
            Diagnostics.DiagnosticHelper.Logger.Debug(Diagnostics.DiagnosticsEventIds.ModuleBuildEvent, "******************************");
        }
        
        /// <summary>
        /// Stops the engine modules.
        /// </summary>
        public void Dispose()
        {
            this.Stop();
        }
        
        /// <summary>
        /// Start the runtime.
        /// </summary>
        private void StartCore()
        {
            bool flag = false;
            RuntimeAppConfigHelper.ApplyConfiguration(RuntimeConfiguration.Current.EngineAppConfigFilePath);
            foreach (RuntimeModule module in this.Modules)
            {
                if (!flag)
                {
                    Diagnostics.DiagnosticHelper.Logger.Debug(Diagnostics.DiagnosticsEventIds.ModuleStartEvent, "****** STARTING MODULES ******");
                    flag = true;
                }
                
                module.Start();
            }
            
            Diagnostics.DiagnosticHelper.Logger.Debug(Diagnostics.DiagnosticsEventIds.ModuleBuildEvent, "******************************");
        }
    }
}
