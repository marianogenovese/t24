//-----------------------------------------------------------------------
// <copyright file="OperationDispatcherModule.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// This class implements the Integra Vision Engine Module.
    /// </summary>
    internal sealed class OperationDispatcherModule : Module, IOperationDispatcherModule
    {
        /// <summary>
        /// Used for execute the command.
        /// </summary>
        private IEngine engine;

        /// <summary>
        /// Pipeline factory builder.
        /// </summary>
        private DispatchPipelineFactoryBuilder pipelineFactoryBuilder;

        /// <summary>
        /// Pipeline factory.
        /// </summary>
        private DispatchFilterFactory<OperationContext, OperationContext> pipelineFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationDispatcherModule"/> class.
        /// </summary>
        /// <param name="engine">The engine used for executing the command.</param>
        public OperationDispatcherModule(IEngine engine)
        {
            Contract.Requires(engine != null);
            this.engine = engine;
            this.pipelineFactoryBuilder = new DispatchPipelineFactoryBuilder();
        }

        /// <summary>
        /// This method implement a dispatching logic using a pipeline pattern.
        /// </summary>
        /// <param name="context">The context to process.</param>
        /// <returns>A operator that represent the worker of the dispatching action.</returns>
        public DispatchOperator Dispatch(OperationContext context)
        {
            return new DispatchOperator(context, this.pipelineFactory.Create());
        }

        /// <summary>
        /// Abort the module.
        /// </summary>
        protected override void OnAbort()
        {
        }

        /// <summary>
        /// Stop the module.
        /// </summary>
        protected override void OnStop()
        {
        }

        /// <summary>
        /// Start the module.
        /// </summary>
        protected override void OnStart()
        {
            this.pipelineFactory = this.pipelineFactoryBuilder.Build();
        }
    }
}
