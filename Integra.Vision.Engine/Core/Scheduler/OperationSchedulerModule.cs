//-----------------------------------------------------------------------
// <copyright file="OperationSchedulerModule.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// This class implements the Integra Vision Engine Module.
    /// </summary>
    internal sealed class OperationSchedulerModule : Module, IOperationSchedulerModule
    {
        /// <summary>
        /// The low level scheduler used for queue tasks.
        /// </summary>
        private static TaskScheduler lowLevelScheduler = TaskScheduler.Default;

        /// <summary>
        /// The Task factory.
        /// </summary>
        private static TaskFactory taskFactory = new TaskFactory(lowLevelScheduler);

        /// <summary>
        /// Used for execute the command when a request arrive to the queue.
        /// </summary>
        private IOperationDispatcherModule dispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationSchedulerModule"/> class.
        /// </summary>
        /// <param name="dispatcher">Used for executing the command.</param>
        public OperationSchedulerModule(IOperationDispatcherModule dispatcher)
        {
            Contract.Requires(dispatcher != null);
            this.dispatcher = dispatcher;
        }

        /// <inheritdoc />
        public void Schedule(OperationContext context)
        {
            // Se calendariza el despacho de la operación
            taskFactory.StartNew(
                () =>
                {
                    DispatchOperator dispatchoperator = this.dispatcher.Dispatch(context);
                    dispatchoperator.Execute();
                });
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
        }
    }
}
