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
        private readonly Uri address;

        /// <summary>
        /// Listener address.
        /// </summary>
        private readonly CommandListenerBinding.MessageEncoding encodingType;

        /// <summary>
        /// The queue used for enqueue requests.
        /// </summary>
        private readonly IOperationSchedulerModule queue;

        /// <summary>
        /// The authenticator of the user.
        /// </summary>
        private readonly IUserAuthenticator userAuthenticator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandListenerServiceFactory"/> class.
        /// </summary>
        /// <param name="address">The listener address.</param>
        /// <param name="encodingType">The type of encoding used.</param>
        /// <param name="queue">The queue used for enqueue requests.</param>
        /// <param name="userAuthenticator">The authenticator of the user.</param>
        public CommandListenerServiceFactory(Uri address, CommandListenerBinding.MessageEncoding encodingType, IOperationSchedulerModule queue, IUserAuthenticator userAuthenticator)
        {
            this.address = address;
            this.encodingType = encodingType;
            this.queue = queue;
            this.userAuthenticator = userAuthenticator;
        }

        /// <inheritdoc />
        public override ServiceHostBase CreateServiceHost(string constructorString, System.Uri[] baseAddresses)
        {
            return new CommandListenerServiceHost(this.address, this.encodingType, this.queue, this.userAuthenticator);
        }
    }
}
