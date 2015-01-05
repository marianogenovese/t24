//-----------------------------------------------------------------------
// <copyright file="CommandListenerSecurityTokenAuthenticator.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IdentityModel.Claims;
    using System.IdentityModel.Policy;
    using System.IdentityModel.Selectors;
    using System.Security.Principal;
    
    /// <summary>
    /// The command listener user/password validator.
    /// </summary>
    internal sealed class CommandListenerSecurityTokenAuthenticator : UserNameSecurityTokenAuthenticator
    {
        /// <summary>
        /// Policy expiration.
        /// </summary>
        private readonly DateTime expiration = DateTime.MaxValue.ToUniversalTime();

        /// <summary>
        /// The authenticator of users.
        /// </summary>
        private readonly IUserAuthenticator userAuthenticator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandListenerSecurityTokenAuthenticator"/> class.
        /// </summary>
        /// <param name="userAuthenticator">The authenticator of the user.</param>
        public CommandListenerSecurityTokenAuthenticator(IUserAuthenticator userAuthenticator)
        {
            this.userAuthenticator = userAuthenticator;
        }

        /// <inheritdoc />
        protected override ReadOnlyCollection<IAuthorizationPolicy> ValidateUserNamePasswordCore(string userName, string password)
        {
            List<Claim> identityClaims = new List<Claim>();
            List<System.Security.Claims.Claim> securityClaims = new List<System.Security.Claims.Claim>();
            
            if (this.userAuthenticator.Validate(userName, password))
            {
                securityClaims.Add(new System.Security.Claims.Claim(EngineClaimTypes.ConnectionAllowed, userName));
                securityClaims.Add(new System.Security.Claims.Claim(EngineClaimTypes.CreateAssemblyClaim, string.Empty));

                identityClaims.Add(new Claim(EngineClaimTypes.ConnectionAllowed, true, Rights.PossessProperty));
            }

            IIdentity identity = new VisionUserIdentity(userName, securityClaims.ToArray());
            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
            policies.Add(new CommandListenerAuthorizationPolicy(ClaimSet.System, new DefaultClaimSet(identityClaims.ToArray()), this.expiration, identity));
            return policies.AsReadOnly();
        }
    }
}
