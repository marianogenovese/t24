//-----------------------------------------------------------------------
// <copyright file="CommandListenerBinding.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Security.Tokens;
    
    /// <summary>
    /// Provides self configured binding for the command listener.
    /// </summary>
    internal sealed class CommandListenerBinding : CustomBinding
    {
        /// <summary>
        /// The custom binding collection used in this binding.
        /// </summary>
        private CommandListenerBindingElement[] bindings;

        /// <summary>
        /// Collection of binding elements used for service model.
        /// </summary>
        private BindingElementCollection bindingElementCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandListenerBinding"/> class.
        /// </summary>
        /// <param name="encodingType">The encoding type used.</param>
        public CommandListenerBinding(MessageEncoding encodingType)
        {
            this.bindings = new CommandListenerBindingElement[]
            {
                new CommandListenerSecurityBindingElement(),
                new CommandListenerMessageEncodingBindingElement(encodingType),
                new CommandListenerTransportBindingElement()
            };
        }
        
        /// <summary>
        /// Types of message encoding.
        /// </summary>
        internal enum MessageEncoding
        {
            /// <summary>
            /// Use the binary encoding.
            /// </summary>
            Binary,
            
            /// <summary>
            /// Use the text encoding.
            /// </summary>
            Text
        }

        /// <summary>
        /// Scheme associated to this binding.
        /// </summary>
        public override string Scheme
        {
            get
            {
                return "net.tcp";
            }
        }
        
        /// <inheritdoc />
        public override BindingElementCollection CreateBindingElements()
        {
            if (this.bindingElementCollection != null)
            {
                return this.bindingElementCollection; 
            }

            this.bindingElementCollection = new BindingElementCollection();
            foreach (CommandListenerBindingElement element in this.bindings)
            {
                this.bindingElementCollection.Add(element.Clone());
            }

            return this.bindingElementCollection;
        }

        /// <summary>
        /// This class represents a binding element that contains a inner binding element that cannot be inherit.
        /// </summary>
        private abstract class CommandListenerBindingElement : BindingElement
        {
            /// <summary>
            /// Used for mutual exclusive lock.
            /// </summary>
            private object lockThis = new object();

            /// <summary>
            /// Flag used for initialize this binding.
            /// </summary>
            private bool initialized = false;
            
            /// <summary>
            /// The inner binding element that this class use.
            /// </summary>
            private BindingElement innerBindingElement;
            
            /// <inheritdoc />
            public override BindingElement Clone()
            {
                this.CheckInitialization();
                return this.innerBindingElement.Clone();
            }

            /// <inheritdoc />
            public override T GetProperty<T>(BindingContext context)
            {
                this.CheckInitialization();
                return this.innerBindingElement.GetProperty<T>(context);
            }

            /// <inheritdoc />
            public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
            {
                this.CheckInitialization();                
                return this.innerBindingElement.BuildChannelFactory<TChannel>(context);
            }

            /// <inheritdoc />
            public override IChannelListener<TChannel> BuildChannelListener<TChannel>(BindingContext context)
            {
                this.CheckInitialization();
                return this.innerBindingElement.BuildChannelListener<TChannel>(context);
            }

            /// <inheritdoc />
            public override bool CanBuildChannelFactory<TChannel>(BindingContext context)
            {
                this.CheckInitialization();
                return this.innerBindingElement.CanBuildChannelFactory<TChannel>(context);
            }

            /// <inheritdoc />
            public override bool CanBuildChannelListener<TChannel>(BindingContext context)
            {
                this.CheckInitialization();
                return this.innerBindingElement.CanBuildChannelListener<TChannel>(context);
            }
            
            /// <summary>
            /// This method allow to implements binding element initialization logic.
            /// </summary>
            /// <returns>The new binding element</returns>
            protected abstract BindingElement Initialize();

            /// <summary>
            /// Check for binding initialization.
            /// </summary>
            private void CheckInitialization()
            {
                lock (this.lockThis)
                {
                    if (!this.initialized)
                    {
                        this.innerBindingElement = this.Initialize();
                        this.initialized = true;
                    }
                }
            }
        }

        /// <summary>
        /// This class define a security binding element for the listener.
        /// </summary>
        private sealed class CommandListenerSecurityBindingElement : CommandListenerBindingElement
        {
            /// <inheritdoc />
            protected override BindingElement Initialize()
            {
                SymmetricSecurityBindingElement innerBindingElement = new SymmetricSecurityBindingElement();
                innerBindingElement.DefaultAlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.Basic256;
                innerBindingElement.IncludeTimestamp = true;
                innerBindingElement.KeyEntropyMode = System.ServiceModel.Security.SecurityKeyEntropyMode.CombinedEntropy;
                innerBindingElement.MessageSecurityVersion = System.ServiceModel.MessageSecurityVersion.Default;
                innerBindingElement.SecurityHeaderLayout = SecurityHeaderLayout.Strict;
                innerBindingElement.ProtectTokens = false;
                innerBindingElement.MessageProtectionOrder = System.ServiceModel.Security.MessageProtectionOrder.SignBeforeEncryptAndEncryptSignature;
                innerBindingElement.RequireSignatureConfirmation = false;
                innerBindingElement.ProtectionTokenParameters = this.CreateProtectionTokenParameters();
                
                // return SecurityBindingElement.CreateSecureConversationBindingElement(SecurityBindingElement.CreateUserNameOverTransportBindingElement());
                return innerBindingElement;
            }

            /// <summary>
            /// Create parameters for token protection.
            /// </summary>
            /// <returns>The security token parameters.</returns>
            private SecureConversationSecurityTokenParameters CreateProtectionTokenParameters()
            {
                SecureConversationSecurityTokenParameters parameters = new SecureConversationSecurityTokenParameters();
                parameters.InclusionMode = SecurityTokenInclusionMode.AlwaysToRecipient;
                parameters.ReferenceStyle = SecurityTokenReferenceStyle.Internal;
                parameters.RequireDerivedKeys = true;
                parameters.RequireCancellation = true;
                parameters.BootstrapSecurityBindingElement = this.CreateBootstrapSecurityBindingElement();
                return parameters;
            }

            /// <summary>
            /// Create a bootstrap security binding element.
            /// </summary>
            /// <returns>The new binding element.</returns>
            private SymmetricSecurityBindingElement CreateBootstrapSecurityBindingElement()
            {
                SymmetricSecurityBindingElement bootstrapSecurityBindingElement = new SymmetricSecurityBindingElement();
                bootstrapSecurityBindingElement.DefaultAlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.Basic256;
                bootstrapSecurityBindingElement.IncludeTimestamp = true;
                bootstrapSecurityBindingElement.KeyEntropyMode = System.ServiceModel.Security.SecurityKeyEntropyMode.CombinedEntropy;
                bootstrapSecurityBindingElement.MessageSecurityVersion = System.ServiceModel.MessageSecurityVersion.WSSecurity11WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11;
                bootstrapSecurityBindingElement.SecurityHeaderLayout = SecurityHeaderLayout.Strict;
                bootstrapSecurityBindingElement.ProtectTokens = false;
                bootstrapSecurityBindingElement.EndpointSupportingTokenParameters.SignedEncrypted.Add(new System.ServiceModel.Security.Tokens.UserNameSecurityTokenParameters());
                bootstrapSecurityBindingElement.EndpointSupportingTokenParameters.SignedEncrypted[0].InclusionMode = SecurityTokenInclusionMode.AlwaysToRecipient;
                bootstrapSecurityBindingElement.EndpointSupportingTokenParameters.SignedEncrypted[0].ReferenceStyle = SecurityTokenReferenceStyle.Internal;
                bootstrapSecurityBindingElement.EndpointSupportingTokenParameters.SignedEncrypted[0].RequireDerivedKeys = false;

                bootstrapSecurityBindingElement.MessageProtectionOrder = System.ServiceModel.Security.MessageProtectionOrder.SignBeforeEncryptAndEncryptSignature;
                bootstrapSecurityBindingElement.RequireSignatureConfirmation = false;
                bootstrapSecurityBindingElement.ProtectionTokenParameters = new SslSecurityTokenParameters();
                bootstrapSecurityBindingElement.ProtectionTokenParameters.InclusionMode = SecurityTokenInclusionMode.AlwaysToRecipient;
                bootstrapSecurityBindingElement.ProtectionTokenParameters.ReferenceStyle = SecurityTokenReferenceStyle.Internal;
                bootstrapSecurityBindingElement.ProtectionTokenParameters.RequireDerivedKeys = true;
                ((SslSecurityTokenParameters)bootstrapSecurityBindingElement.ProtectionTokenParameters).RequireCancellation = true;
                ((SslSecurityTokenParameters)bootstrapSecurityBindingElement.ProtectionTokenParameters).RequireClientCertificate = false;
                return bootstrapSecurityBindingElement;
            }
        }

        /// <summary>
        /// This class implements the message encoding binding for the Integra Vision Engine Command Listener.
        /// </summary>
        private sealed class CommandListenerMessageEncodingBindingElement : CommandListenerBindingElement
        {
            /// <summary>
            /// The encoding type used in this binding.
            /// </summary>
            private MessageEncoding encodingType;

            /// <summary>
            /// Initializes a new instance of the <see cref="CommandListenerMessageEncodingBindingElement"/> class.
            /// </summary>
            /// <param name="encodingType">The type of encoding used.</param>
            public CommandListenerMessageEncodingBindingElement(MessageEncoding encodingType)
            {
                this.encodingType = encodingType;
            }
            
            /// <inheritdoc />
            protected override BindingElement Initialize()
            {
                if (this.encodingType == MessageEncoding.Binary)
                {
                    return new BinaryMessageEncodingBindingElement();
                }
                else
                {
                    return new TextMessageEncodingBindingElement();
                }
            }
        }

        /// <summary>
        /// This class implements the transport binding for the Integra Vision Engine Command Listener.
        /// </summary>
        private sealed class CommandListenerTransportBindingElement : CommandListenerBindingElement
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CommandListenerTransportBindingElement"/> class.
            /// </summary>
            public CommandListenerTransportBindingElement()
            {
            }

            /// <inheritdoc />
            protected override BindingElement Initialize()
            {
                return new TcpTransportBindingElement();
            }
        }
    }
}
