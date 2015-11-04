//-----------------------------------------------------------------------
// <copyright file="CommandListenerServiceHost.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;
    
    /// <summary>
    /// Provides a self configured service host used as listener for request commands.
    /// </summary>
    internal sealed class CommandListenerServiceHost : ServiceHost
    {
        /// <summary>
        /// Listener address.
        /// </summary>
        private readonly Uri address;
        
        /// <summary>
        /// Listener address.
        /// </summary>
        private readonly Integra.Vision.Engine.Core.CommandListenerBinding.MessageEncoding encodingType;

        /// <summary>
        /// The queue used for enqueue requests.
        /// </summary>
        private readonly IOperationSchedulerModule queue;

        /// <summary>
        /// The authenticator of the user.
        /// </summary>
        private readonly IUserAuthenticator userAuthenticator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandListenerServiceHost"/> class.
        /// </summary>
        /// <param name="address">The listener address.</param>
        /// <param name="encodingType">The type of encoding used.</param>
        /// <param name="queue">The queue used for enqueue requests.</param>
        /// <param name="userAuthenticator">The authenticator of the user.</param>
        public CommandListenerServiceHost(Uri address, Integra.Vision.Engine.Core.CommandListenerBinding.MessageEncoding encodingType, IOperationSchedulerModule queue, IUserAuthenticator userAuthenticator) : base(typeof(CommandRequestHandler), new Uri[] { address })
        {
            this.address = address;
            this.encodingType = encodingType;
            this.queue = queue;
            this.userAuthenticator = userAuthenticator;
        }

        /// <summary>
        /// This service host skip the configuration by app.config.
        /// </summary>
        protected override void ApplyConfiguration()
        {
            // base.ApplyConfiguration();
        }

        /// <summary>
        /// Configure service throttle.
        /// </summary>
        protected override void OnOpened()
        {
            base.OnOpened();

            // ChannelDispatcher dispatcher = new ChannelDispatcher(this.ChannelDispatchers[0].Listener);
            // dispatcher.ServiceThrottle.MaxConcurrentCalls = int.MaxValue;
            // dispatcher.ServiceThrottle.MaxConcurrentInstances = int.MaxValue;
            // dispatcher.ServiceThrottle.MaxConcurrentSessions = int.MaxValue;
        }
        
        /// <summary>
        /// Initialize the listener.
        /// </summary>
        protected override void InitializeRuntime()
        {
            this.ConfigureServiceThrottling();
            this.ConfigureEndpoint();
            this.ConfigureCertificate();
            this.ConfigureCredentialsBehavior();
            this.ConfigureAuthorization();
            this.ConfigureDebugBehaviour();
            this.ConfigureInstanceProvider();
            this.ConfigureOperationBehaviors();
            this.ConfigureTypeResolver();
            base.InitializeRuntime();
        }

        /// <summary>
        /// Doc goes here
        /// </summary>
        private void ConfigureServiceThrottling()
        {
            ServiceThrottlingBehavior behavior = this.Description.Behaviors.Find<ServiceThrottlingBehavior>();
            if (behavior == null)
            {
                behavior = new ServiceThrottlingBehavior();
                this.Description.Behaviors.Add(behavior);
            }
            
            behavior = this.Description.Behaviors.Find<ServiceThrottlingBehavior>();
            
            behavior.MaxConcurrentCalls = 1000;
            behavior.MaxConcurrentSessions = 1000;
            behavior.MaxConcurrentInstances = 1000;
        }

        /// <summary>
        /// Add the endpoint used in this service.
        /// </summary>
        private void ConfigureEndpoint()
        {
            this.AddServiceEndpoint(typeof(ICommandRequestHandler), new CommandListenerBinding(this.encodingType), string.Empty);
        }

        /// <summary>
        /// Configure the service certificate.
        /// </summary>
        private void ConfigureCertificate()
        {
            this.Credentials.ServiceCertificate.Certificate = CommandListenerCertificate.Current;
        }

        /// <summary>
        /// Configure the service credentials behavior.
        /// </summary>
        private void ConfigureCredentialsBehavior()
        {
            Description.Behaviors.Remove<ServiceCredentials>();
            CommandListenerServiceCredentials serviceCredentials = new CommandListenerServiceCredentials(this.userAuthenticator);
            serviceCredentials.ServiceCertificate.Certificate = CommandListenerCertificate.Current;
            Description.Behaviors.Add(serviceCredentials);
            Credentials.UseIdentityConfiguration = false;
            Credentials.UserNameAuthentication.UserNamePasswordValidationMode = System.ServiceModel.Security.UserNamePasswordValidationMode.Custom;
        }

        /// <summary>
        /// Configure the service authorization.
        /// </summary>
        private void ConfigureAuthorization()
        {
            this.Authorization.ServiceAuthorizationManager = new CommandListenerAuthorizationManager();
            this.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
        }

        /// <summary>
        /// Configure the service authorization.
        /// </summary>
        private void ConfigureDebugBehaviour()
        {
            ServiceDebugBehavior behavior = this.Description.Behaviors.Find<ServiceDebugBehavior>();
            if (behavior == null)
            {
                behavior = new ServiceDebugBehavior();
                behavior.IncludeExceptionDetailInFaults = true;
                this.Description.Behaviors.Add(behavior);
            }
            else
            {
                behavior.IncludeExceptionDetailInFaults = true;
            }
        }

        /// <summary>
        /// Configure the instance provider.
        /// </summary>
        private void ConfigureInstanceProvider()
        {
            foreach (var contract in this.ImplementedContracts.Values)
            {
                contract.Behaviors.Add(new CommandRequestHandlerInstanceProvider(this.queue));
            }
        }

        /// <summary>
        /// Configure the operation behaviors.
        /// </summary>
        private void ConfigureOperationBehaviors()
        {
            foreach (ServiceEndpoint endpoint in Description.Endpoints)
            {
                foreach (OperationDescription operation in endpoint.Contract.Operations)
                {
                    if (!operation.OperationBehaviors.Contains(typeof(CommandListenerOperationBehavior)))
                    {
                        operation.OperationBehaviors.Insert(2, new CommandListenerOperationBehavior());
                    }
                }
            }
        }

        /// <summary>
        /// Configure the operation behaviors.
        /// </summary>
        private void ConfigureTypeResolver()
        {
            foreach (ServiceEndpoint endpoint in Description.Endpoints)
            {
                foreach (OperationDescription operation in endpoint.Contract.Operations)
                {
                    DataContractSerializerOperationBehavior serializerBehavior = operation.Behaviors.Find<DataContractSerializerOperationBehavior>();

                    if (serializerBehavior == null)
                    {
                        serializerBehavior = new DataContractSerializerOperationBehavior(operation);
                        operation.Behaviors.Add(serializerBehavior);
                    }

                    serializerBehavior.DataContractResolver = new CommandListenerTypeResolver();
                }
            }
        }
    }
}
