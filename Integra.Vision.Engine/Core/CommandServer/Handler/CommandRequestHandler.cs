//-----------------------------------------------------------------------
// <copyright file="CommandRequestHandler.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Threading;
    using System.Threading.Tasks;

    /// <inheritdoc />
    internal sealed class CommandRequestHandler : ICommandRequestHandler
    {
        /// <summary>
        /// The queue where the requests are enqueued for future processing.
        /// </summary>
        private IOperationSchedulerModule scheduler;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandRequestHandler"/> class.
        /// </summary>
        /// <param name="scheduler">The command queue where the requests are enqueued.</param>
        public CommandRequestHandler(IOperationSchedulerModule scheduler)
        {
            this.scheduler = scheduler;
        }

        /// <inheritdoc />
        public Task<OperationResponse> Handle(OperationRequest request)
        {
            return this.Process(request);
        }

        /// <summary>
        /// Process the current request and schedule the execution.
        /// </summary>
        /// <param name="request">The client request.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task<OperationResponse> Process(OperationRequest request)
        {
            /*
             * Se crea un contexto para la nueva operación requerida,
             * se toma del CurrentPrincipal el objeto que representa al usuario que viene del lado del cliente.
             */
            OperationContext context = new OperationContextWrapper(Thread.CurrentPrincipal, request);
            this.scheduler.Schedule(context); // Se agenda la ejecución del contexto.
            await context.WaitForCompletion(); // Se espera por la terminación de la operación.
            return context.Response;
        }
    }
}
