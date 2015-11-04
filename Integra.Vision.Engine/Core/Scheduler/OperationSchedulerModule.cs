//-----------------------------------------------------------------------
// <copyright file="OperationSchedulerModule.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using System.Threading.Tasks.Schedulers;

    /// <summary>
    /// This class implements the Integra Vision Engine Module.
    /// </summary>
    internal sealed class OperationSchedulerModule : Module, IOperationSchedulerModule
    {
        /// <summary>
        /// Transaction counter
        /// </summary>
        private static long contador = 0;

        /// <summary>
        /// The low level scheduler used for queue tasks.
        /// </summary>
        private static TaskScheduler lowLevelScheduler = new WorkStealingTaskScheduler(20);

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
            CommandRequestHandler.t.Change(0, 1000);
        }

        /// <inheritdoc />
        public void Schedule(OperationContext context)
        {
            taskFactory.StartNew(
                () =>
                {
                    DispatchOperator dispatchoperator = this.dispatcher.Dispatch(context);
                    dispatchoperator.Execute();
                });
        }

        /// <inheritdoc />
        public void Schedule(string sourceName, Event.EventObject eventObject)
        {
            if (++contador % 1000 == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Entran: {0}", contador);
                Console.ResetColor();
            }

            if (Sources.GetSource(sourceName) != null)
            {
                Sources.GetSource(sourceName).Post(eventObject);
            }
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
