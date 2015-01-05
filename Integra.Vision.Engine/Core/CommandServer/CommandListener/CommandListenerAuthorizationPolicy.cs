//-----------------------------------------------------------------------
// <copyright file="CommandListenerAuthorizationPolicy.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IdentityModel.Claims;
    using System.IdentityModel.Policy;
    using System.Security.Claims;
    using System.Security.Principal;
    
    /// <summary>
    /// The command listener authorization policy.
    /// </summary>
    internal sealed class CommandListenerAuthorizationPolicy : IAuthorizationPolicy
    {
        /// <summary>
        /// The id of the policy
        /// </summary>
        private string id = Guid.NewGuid().ToString();
        
        /// <summary>
        /// The issuer of the policy
        /// </summary>
        private ClaimSet issuer;
        
        /// <summary>
        /// The issuance of the policy
        /// </summary>
        private ClaimSet issuance;
        
        /// <summary>
        /// The expirationTime of the policy
        /// </summary>
        private DateTime expirationTime;
        
        /// <summary>
        /// The identities of the policy
        /// </summary>
        private IIdentity identity;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandListenerAuthorizationPolicy"/> class.
        /// </summary>
        /// <param name="issuer">The issuer of the authorization policy.</param>
        /// <param name="issuance">The issuance of the authorization policy.</param>
        /// <param name="expirationTime">The expirationTime of the authorization policy.</param>
        /// <param name="identity">The identity of the authorization policy.</param>
        public CommandListenerAuthorizationPolicy(ClaimSet issuer, ClaimSet issuance, DateTime expirationTime, IIdentity identity)
        {
            Contract.Requires(issuer != null);
            Contract.Requires(issuance != null);
            this.issuer = issuer;
            this.issuance = issuance;
            this.identity = identity;
            this.expirationTime = expirationTime;
        }

        /// <summary>
        /// Gets the policy identification.
        /// </summary>
        public string Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// Gets the issuer of the policy.
        /// </summary>
        public ClaimSet Issuer
        {
            get
            {
                return this.issuer;
            }
        }

        /// <summary>
        /// Gets the expiration of the token.
        /// </summary>
        public DateTime ExpirationTime
        {
            get
            {
                return this.expirationTime;
            }
        }

        /// <inheritdoc />
        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
            evaluationContext.AddClaimSet(this, this.issuance);
            evaluationContext.Properties["Principal"] = new ClaimsPrincipal(this.identity); 
            evaluationContext.RecordExpirationTime(this.expirationTime);
            return true;
        }
    }
}
