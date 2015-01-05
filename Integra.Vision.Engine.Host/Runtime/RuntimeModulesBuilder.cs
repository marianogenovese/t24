//-----------------------------------------------------------------------
// <copyright file="RuntimeModulesBuilder.cs" company="Integra.Vision.Engine.Host">
//     Copyright (c) Integra.Vision.Engine.Host. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Host.Runtime
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Integra.Vision.Dependency;
    using Integra.Vision.Engine.Host.Configuration;
    
    /// <summary>
    /// This class implements a module runtime build method.
    /// </summary>
    internal sealed class RuntimeModulesBuilder
    {
        /// <summary>
        /// The modules dependency resolver.
        /// </summary>
        private IDependencyResolver dependencyResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeModulesBuilder"/> class.
        /// </summary>
        /// <param name="dependencyResolver">Used for resolve the module dependencies.</param>
        public RuntimeModulesBuilder(IDependencyResolver dependencyResolver)
        {
            Contract.Requires(dependencyResolver != null);
            this.dependencyResolver = dependencyResolver;
        }

        /// <summary>
        /// Use reflection to load the modules used in the engine.
        /// </summary>
        /// <returns>The modules.</returns>
        public RuntimeModule[] Build()
        {
            /*
             * Este metodo debe:
             * 1. Buscar en la configuración el resolutor de dependencias.
             * 2. Generar una nueva instancia del resolutor.
             * 3. De la configuración obtener los modulos que debe resolver y pedirselos al resolutor.
             * 4. Crear por cada modulo un IRuntimeModule que permita controlar dinamicamente el inicio de cada modulo.
             */

            Diagnostics.DiagnosticHelper.Logger.Debug(Diagnostics.DiagnosticsEventIds.ModuleBuildEvent, "****** BUILDING MODULES ******");
            RuntimeAppConfigHelper.RestoreConfiguration();
            
            // Se crea una instancia del engine builder que es el encargado de que en sistema del motor, se registren            
            RuntimeEngineBuilder engineBuilder = default(RuntimeEngineBuilder);
            if (!RuntimeObjectWrapperResolver.TryResolve<RuntimeEngineBuilder>(RuntimeConfiguration.Current.EngineBuilderType, out engineBuilder))
            {
                throw new RuntimeException(Resources.SR.UnableCreateInstanceOf(RuntimeConfiguration.Current.EngineBuilderType));
            }
            
            RuntimeAppConfigHelper.ApplyConfiguration(RuntimeConfiguration.Current.EngineAppConfigFilePath);
            engineBuilder.Build(this.dependencyResolver);
            RuntimeAppConfigHelper.RestoreConfiguration();

            List<RuntimeModuleBuilder> modulesBuilder = new List<RuntimeModuleBuilder>();
            foreach (ModuleConfigurationElement module in RuntimeConfiguration.Current.HostingConfiguration.Bootstrap)
            {
                RuntimeModuleBuilder newModuleBuilder = default(RuntimeModuleBuilder);
                if (!RuntimeObjectWrapperResolver.TryResolve<RuntimeModuleBuilder>(this.dependencyResolver, module.Type, out newModuleBuilder))
                {
                    throw new RuntimeException(Resources.SR.UnresolvedModuleDependency(module.Type));
                }

                modulesBuilder.Add(newModuleBuilder);
            }

            List<RuntimeModule> modules = new List<RuntimeModule>();
            RuntimeAppConfigHelper.ApplyConfiguration(RuntimeConfiguration.Current.EngineAppConfigFilePath);
            foreach (RuntimeModuleBuilder builder in modulesBuilder)
            {
                RuntimeModule newModule = default(RuntimeModule);
                newModule = builder.Build();
                modules.Add(newModule);
            }
            
            RuntimeAppConfigHelper.RestoreConfiguration();
            Diagnostics.DiagnosticHelper.Logger.Debug(Diagnostics.DiagnosticsEventIds.ModuleBuildEvent, "******************************");
            return modules.ToArray();
        }

        /// <summary>
        /// This class represent a engine module builder
        /// </summary>
        private class RuntimeModuleBuilder : RuntimeObjectWrapper
        {        
            /// <summary>
            /// Initializes a new instance of the <see cref="RuntimeModuleBuilder"/> class.
            /// </summary>
            /// <param name="instance">The engine module builder instance.</param>
            public RuntimeModuleBuilder(object instance) : base(instance)
            {
            }
            
            /// <summary>
            /// Create a Runtime Module Builder and invoke the building function to create a module.
            /// </summary>
            /// <returns>A Runtime Module.</returns>
            public RuntimeModule Build()
            {
                object moduleInstance = default(object);
                Diagnostics.DiagnosticHelper.Logger.Debug(Diagnostics.DiagnosticsEventIds.ModuleBuildEvent, string.Format("Building a module using '{0}'...", this.Instance.GetType().FullName));
                if (!this.TryInvokeFunction("Build", out moduleInstance))
                {
                    throw new RuntimeException(Resources.SR.UnableToBuildModule(this.Instance.GetType().FullName));
                }

                Diagnostics.DiagnosticHelper.Logger.Debug(Diagnostics.DiagnosticsEventIds.ModuleBuildEvent, string.Format("'{0}' Module has been builded.", moduleInstance.GetType().FullName));
                return new RuntimeModule(moduleInstance);
            }
        }
    }
}