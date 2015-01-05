//-----------------------------------------------------------------------
// <copyright file="CommandServerModule.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Diagnostics.Contracts;
    using System.ServiceModel;
    using Integra.Vision.Engine.Configuration;

    /// <summary>
    /// This class implements the Integra Vision Engine Module.
    /// </summary>
    internal sealed class CommandServerModule : Module, ICommandServerModule
    {
        /// <summary>
        /// The service host for command listener;
        /// </summary>
        private ServiceHostBase commandListener;

        /// <summary>
        /// Used for schedule the request of execute a command.
        /// </summary>
        private IOperationSchedulerModule queue;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandServerModule"/> class.
        /// </summary>
        /// <param name="queue">Where the incoming requests are scheduled.</param>
        /// <param name="userAuthenticator">The authenticator of the user.</param>
        public CommandServerModule(IOperationSchedulerModule queue, IUserAuthenticator userAuthenticator)
        {
            Contract.Requires(queue != null);
            this.queue = queue;
            CommandListenerServiceFactory factory = new CommandListenerServiceFactory(new Uri(EngineConfiguration.Current.DefaultEndpoint.Address), CommandListenerBinding.MessageEncoding.Text, queue, userAuthenticator);
            this.commandListener = factory.CreateServiceHost(null);
        }

        /// <summary>
        /// Abort the module.
        /// </summary>
        protected override void OnAbort()
        {
            this.commandListener.Abort();
        }

        /// <summary>
        /// Stop the module.
        /// </summary>
        protected override void OnStop()
        {
            this.commandListener.Close();
        }

        /// <summary>
        /// Start the module.
        /// </summary>
        protected override void OnStart()
        {
            this.commandListener.Open();
        }
    }
}
