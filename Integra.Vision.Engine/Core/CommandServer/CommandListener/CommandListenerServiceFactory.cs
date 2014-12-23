//-----------------------------------------------------------------------
// <copyright file="CommandListenerServiceFactory.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Activation;

    /// <summary>
    /// Provides a factory for creating new listeners of request commands.
    /// </summary>
    internal sealed class CommandListenerServiceFactory : ServiceHostFactoryBase
    {
        /// <summary>
        /// Listener address.
        /// </summary>
        private Uri address;

        /// <summary>
        /// Listener address.
        /// </summary>
        private CommandListenerBinding.MessageEncoding encodingType;

        /// <summary>
        /// The queue used for enqueue requests.
        /// </summary>
        private IOperationSchedulerModule queue;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandListenerServiceFactory"/> class.
        /// </summary>
        /// <param name="address">The listener address.</param>
        /// <param name="encodingType">The type of encoding used.</param>
        /// <param name="queue">The queue used for enqueue requests.</param>
        public CommandListenerServiceFactory(Uri address, CommandListenerBinding.MessageEncoding encodingType, IOperationSchedulerModule queue)
        {
            this.address = address;
            this.encodingType = encodingType;
            this.queue = queue;
        }

        /// <inheritdoc />
        public override ServiceHostBase CreateServiceHost(string constructorString, System.Uri[] baseAddresses)
        {
            return new CommandListenerServiceHost(this.address, this.encodingType, this.queue);
        }
    }
}
