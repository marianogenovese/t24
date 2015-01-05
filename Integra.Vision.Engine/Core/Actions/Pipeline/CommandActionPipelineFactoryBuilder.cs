//-----------------------------------------------------------------------
// <copyright file="CommandActionPipelineFactoryBuilder.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;
    
    /// <summary>
    /// Allow to build action pipeline factories based on configured steps.
    /// </summary>
    internal sealed class CommandActionPipelineFactoryBuilder
    {
        /// <summary>
        /// Construct, based on configuration, a factory for create new action pipelines.
        /// </summary>
        /// <param name="commandType">The type of the command</param>
        /// <returns>A factory for create new pipelines.</returns>
        public CommandActionFactory Build(CommandTypeEnum commandType)
        {
            object lastInstance = new BeginCommandActionFactory();

            /*
             * En este metodo se resuelvan todas la depedencias en forma de instancias de fabricas de filtros
             * y mediante wrapper invocar el metodo next para encadenar las fabricas para que finalmente se llame
             * al metodo create para devolver una fabrica
             */

            RuntimeActionPipelineFactoryWrapper factory;

            foreach (object instance in CommandActionPipelineFactoryDependencyResolver.Instance.ResolveFactories(commandType))
            {
                if (lastInstance != null)
                {
                    factory = new RuntimeActionPipelineFactoryWrapper(lastInstance);
                    object newInstance = default(object);
                    if (!factory.TryNext(instance, out newInstance))
                    {
                        throw new InvalidOperationException();
                    }

                    lastInstance = newInstance;
                }
                else
                {
                    lastInstance = instance;
                }
            }

            factory = new RuntimeActionPipelineFactoryWrapper(lastInstance);
            object endFilterDispatchFactoryInstance = default(object);
            if (!factory.TryNext(new EndCommandActionFactory(), out endFilterDispatchFactoryInstance))
            {
                throw new InvalidOperationException();
            }

            lastInstance = endFilterDispatchFactoryInstance;

            // Se hace un casteo que aunque es sensible y algo pesado se hace una sola vez
            // ya se tiene lista la fabrica.
            return lastInstance as CommandActionFactory;
        }
        
        /// <summary>
        /// Provides a runtime wrapper for create new action pipeline filters.
        /// </summary>
        private class RuntimeActionPipelineFactoryWrapper : RuntimeObjectWrapper
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="RuntimeActionPipelineFactoryWrapper"/> class.
            /// </summary>
            /// <param name="instance">The real object.</param>
            public RuntimeActionPipelineFactoryWrapper(object instance) : base(instance)
            {
            }

            /// <summary>
            /// Try to link filters.
            /// </summary>
            /// <param name="next">The next filter.</param>
            /// <param name="result">The created filter as result of the linking</param>
            /// <returns>true if the invocation was completed successfully; otherwise, false.</returns>
            public bool TryNext(object next, out object result)
            {
                result = default(object);
                return this.TryInvokeFunction("Next", out result, next);
            }
        }
    }
}