//-----------------------------------------------------------------------
// <copyright file="CommandRequestHandler.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.IO;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Threading;
    using System.Threading.Tasks;

    /// <inheritdoc />
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Single, EnsureOrderedDispatch = true, IgnoreExtensionDataObject = false)]
    internal sealed class CommandRequestHandler : ICommandRequestHandler
    {
        /// <summary>
        /// Counter
        /// </summary>
        private static long contador = 0;

        /// <summary>
        /// Transaction counter
        /// </summary>
        static long counter = 0;

        /// <summary>
        /// Doc goes here
        /// </summary>
        public static Timer t = new Timer(timerCallback, null, 0, 1000);

        /// <summary>
        /// Timer callback
        /// </summary>
        /// <param name="state">State of the timer</param>
        static void timerCallback(object state)
        {
            Console.WriteLine("Received: {0}", Interlocked.Exchange(ref counter, 0));
        }

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
            System.ServiceModel.Dispatcher.ChannelDispatcher dispatcher = System.ServiceModel.OperationContext.Current.Host.ChannelDispatchers[0] as System.ServiceModel.Dispatcher.ChannelDispatcher;
            System.ServiceModel.Dispatcher.ServiceThrottle serviceThrottle = dispatcher.ServiceThrottle;
            return this.Process(request);
        }

        /// <summary>
        /// Doc goes here
        /// </summary>
        /// <param name="request">Operation request</param>
        void ICommandRequestHandler.Receive(OperationRequest request)
        {
            this.ProcessReceive(request);
        }

        /// <summary>
        /// Doc goes here
        /// </summary>
        /// <param name="request">Operation request</param>
        void ICommandRequestHandler.Publish(OperationRequest request)
        {
            this.ProcessPublish(request);
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

            Console.WriteLine("Execute: {0} - {1} - {2}", request.Event.Message[1][3].Value, request.Event.Message[1][2].Value, request.Event.Message[1][1].Value);
            OperationContext context = new OperationContextWrapper(Thread.CurrentPrincipal, request);
            context.Data.Add("IsPrivateCommand", false);
            this.scheduler.Schedule(context); // Se agenda la ejecución del contexto.
            await context.WaitForCompletion(); // Se espera por la terminación de la operación.
            return context.Response;
        }

        /// <summary>
        /// Doc goes here
        /// </summary>
        /// <param name="request">Operation request</param>
        private void ProcessReceive(OperationRequest request)
        {
            try
            {
                OperationContext context = new OperationContextWrapper(Thread.CurrentPrincipal, request, System.ServiceModel.OperationContext.Current);
                context.Data.Add("IsPrivateCommand", false);
                this.scheduler.Schedule(context); // Se agenda la ejecución del contexto.
            }
            catch (Exception e)
            {
                ErrorCommandResult error = new ErrorCommandResult(e);
                throw new FaultException<ErrorCommandResult>(error);
            }
        }

        /// <summary>
        /// Doc goes here
        /// </summary>
        /// <param name="request">Operation request</param>
        private void ProcessPublish(OperationRequest request)
        {
            try
            {
                // Console.WriteLine("Publish: {0} - {1} - {2}", request.Event.Message[1][3].Value, request.Event.Message[1][2].Value, request.Event.Message[1][1].Value);
                if (++contador % 1000 == 0)
                {
                    DateTime datetime = DateTime.Now;
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("Recibí : {0}", contador);
                    Console.ResetColor();
                }

                Interlocked.Increment(ref counter);
                OperationContext context = new OperationContextWrapper(Thread.CurrentPrincipal, request, System.ServiceModel.OperationContext.Current);
                context.Data.Add("IsPrivateCommand", false);
                this.scheduler.Schedule(context); // Se agenda la ejecución del contexto.
            }
            catch (Exception e)
            {
                ErrorCommandResult error = new ErrorCommandResult(e);
                throw new FaultException<ErrorCommandResult>(error);
            }
        }
    }
}
