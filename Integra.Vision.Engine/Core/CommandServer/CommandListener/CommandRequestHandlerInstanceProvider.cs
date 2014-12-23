//-----------------------------------------------------------------------
// <copyright file="CommandRequestHandlerInstanceProvider.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Diagnostics.Contracts;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;
    
    /// <summary>
    /// Provides a provider for command request handlers.
    /// </summary>
    internal sealed class CommandRequestHandlerInstanceProvider : IInstanceProvider, IContractBehavior
    {
        /// <summary>
        /// The dependency used for inject it in the command request handler instance.
        /// </summary>
        private IOperationSchedulerModule queue;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandRequestHandlerInstanceProvider"/> class.
        /// </summary>
        /// <param name="queue">The queue used for inject it in the command request handler instance.</param>
        public CommandRequestHandlerInstanceProvider(IOperationSchedulerModule queue)
        {
            Contract.Requires<ArgumentException>(queue != null);
            this.queue = queue;
        }

        /// <inheritdoc />
        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return this.GetInstance(instanceContext);
        }

        /// <inheritdoc />
        public object GetInstance(InstanceContext instanceContext)
        {
            return new CommandRequestHandler(this.queue);
        }

        /// <inheritdoc />
        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            IDisposable disposable = instance as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        /// <inheritdoc />
        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        /// <inheritdoc />
        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        /// <inheritdoc />
        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
            dispatchRuntime.InstanceProvider = this;
        }

        /// <inheritdoc />
        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
        }
    }
}
