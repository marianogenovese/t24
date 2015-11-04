//-----------------------------------------------------------------------
// <copyright file="CommandListenerOperationBehavior.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Linq;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;
    
    /// <summary>
    /// Modify the command listener service applying a custom transformation/formatting behavior.
    /// </summary>
    internal sealed class CommandListenerOperationBehavior : IOperationBehavior
    {
        /// <inheritdoc />
        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
        }

        /// <inheritdoc />
        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
        }

        /// <inheritdoc />
        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            CommandListenerMessageFormatter commandListenerFormatter = new CommandListenerMessageFormatter();
            dispatchOperation.Formatter = commandListenerFormatter;
        }

        /// <inheritdoc />
        public void Validate(OperationDescription operationDescription)
        {
        }
    }
}
