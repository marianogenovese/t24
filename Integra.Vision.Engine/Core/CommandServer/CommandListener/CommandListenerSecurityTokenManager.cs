//-----------------------------------------------------------------------
// <copyright file="CommandListenerSecurityTokenManager.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.IdentityModel.Selectors;
    using System.IdentityModel.Tokens;
    using System.ServiceModel.Description;
    using System.ServiceModel.Security;
    
    /// <summary>
    /// The security token manager.
    /// </summary>
    internal sealed class CommandListenerSecurityTokenManager : ServiceCredentialsSecurityTokenManager
    {
        /// <summary>
        /// The service credentials configured for the command listener service.
        /// </summary>
        private ServiceCredentials credentials;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandListenerSecurityTokenManager"/> class.
        /// </summary>
        /// <param name="credentials">The service credentials.</param>
        public CommandListenerSecurityTokenManager(ServiceCredentials credentials) : base(credentials)
        {
            this.credentials = credentials;
        }

        /// <inheritdoc />
        public override SecurityTokenAuthenticator CreateSecurityTokenAuthenticator(SecurityTokenRequirement tokenRequirement, out SecurityTokenResolver outOfBandTokenResolver)
        {
            if (tokenRequirement.TokenType == SecurityTokenTypes.UserName)
            {
                outOfBandTokenResolver = null;
                return new CommandListenerSecurityTokenAuthenticator();
            }
            else
            {
                return base.CreateSecurityTokenAuthenticator(tokenRequirement, out outOfBandTokenResolver);
            }
        }
    }
}
