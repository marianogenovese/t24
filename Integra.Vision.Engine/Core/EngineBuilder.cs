//-----------------------------------------------------------------------
// <copyright file="EngineBuilder.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Dependency;
    using Integra.Vision.Engine.Configuration;
    
    /// <summary>
    /// This class implements the builder of the engine used in the bootstrap process.
    /// </summary>
    internal sealed class EngineBuilder : IEngineBuilder
    {
        /// <summary>
        /// This method should contain the registration of the modules necessary to the proper operation of the engine.
        /// </summary>
        /// <param name="dependencyResolver">Used for register the modules.</param>
        public void Build(IDependencyResolver dependencyResolver)
        {
            // Se asigna la instancia que viene al sitema de resolucion de dependencias por defecto del sistema.
            DependencyResolver.Default = dependencyResolver;

            EngineModuleBuilder engineModuleBuilder = new EngineModuleBuilder(DependencyResolver.Default);
            OperationDispatcherModuleBuilder operationHandlerModuleBuilder = new OperationDispatcherModuleBuilder(DependencyResolver.Default);
            OperationSchedulerModuleBuilder taskSchedulerModuleBuilder = new OperationSchedulerModuleBuilder(DependencyResolver.Default);
            BootEngineModuleBuilder privateOperationHandlerModuleBuilder = new BootEngineModuleBuilder(DependencyResolver.Default);
            CommandServerModuleBuilder commandServerModuleBuilder = new CommandServerModuleBuilder(DependencyResolver.Default);

            DependencyResolver.Default.RegisterInstance<EngineModuleBuilder>(engineModuleBuilder);
            DependencyResolver.Default.RegisterInstance<OperationDispatcherModuleBuilder>(operationHandlerModuleBuilder);
            DependencyResolver.Default.RegisterInstance<OperationSchedulerModuleBuilder>(taskSchedulerModuleBuilder);
            DependencyResolver.Default.RegisterInstance<BootEngineModuleBuilder>(privateOperationHandlerModuleBuilder);
            DependencyResolver.Default.RegisterInstance<CommandServerModuleBuilder>(commandServerModuleBuilder);

            /*
             * Esta sección permite validar y registrar las fabricas de filtros para el despacho de requerimientos
             */
            foreach (FilterConfigurationElement filterConfiguration in EngineConfiguration.Current.Operation.Dispatcher)
            {
                Type factoryType = Type.GetType(filterConfiguration.Type);                
                if (factoryType == null || !factoryType.IsDispatchFilterFactory())
                {
                    throw new InvalidOperationException(Integra.Vision.Engine.Resources.SR.InvalidDispatchFilterFactoryType(filterConfiguration.Type));
                }

                DispatchPipelineFactoryDependencyResolver.Instance.RegisterDispatchFilterFactory(filterConfiguration.Name, factoryType);
            }

            /*
             * Esta sección permite registrar las fabricas de filtros para las acciones que se ejecutarán por tipo de requerimiento
             */
            foreach (RuntimeCommandConfigurationElement commandConfiguration in EngineConfiguration.Current.Operation.Actions)
            {
                foreach (FilterConfigurationElement filterConfiguration in commandConfiguration)
                {
                    Type factoryType = Type.GetType(filterConfiguration.Type);
                    if (factoryType == null || !factoryType.IsActionFilterFactory())
                    {
                        throw new InvalidOperationException(Integra.Vision.Engine.Resources.SR.InvalidActionFilterFactoryType(filterConfiguration.Type));
                    }

                    CommandActionPipelineFactoryDependencyResolver.Instance.RegisterActionFilterFactory(commandConfiguration.CommandTypeAsEnum, factoryType);
                }
            }

            dependencyResolver.RegisterInstance<IBufferManager>(new InternalBufferManager(EngineConfiguration.Current.BufferManagement.MaxBufferPoolSize, EngineConfiguration.Current.BufferManagement.MaxBufferSize));

            // Registramos el autenticador de usuarios.
            dependencyResolver.RegisterInstance<IUserAuthenticator>(new DbUserAuthenticator());
        }
    }
}
