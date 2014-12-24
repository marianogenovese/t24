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
            Claim claim = null;
            if (this.userAuthenticator.Validate(userName, password))
            {
                claim = Claim.CreateNameClaim(userName);
            }
            else
            {
                claim = new Claim(Resources.SR.CommandServerServiceContractNameSpace + "InvalidUsernameClaim", true, Rights.PossessProperty);
            }
            
            List<IIdentity> identities = new List<IIdentity>(1);
            identities.Add(new GenericIdentity(userName));
            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
            List<ClaimSet> claimsets = new List<ClaimSet>();
            claimsets.Add(new DefaultClaimSet(claim));
            policies.Add(new CommandListenerAuthorizationPolicy(ClaimSet.System, new DefaultClaimSet(claim), DateTime.MaxValue.ToUniversalTime(), identities));
            return policies.AsReadOnly();
        }
    }
}
