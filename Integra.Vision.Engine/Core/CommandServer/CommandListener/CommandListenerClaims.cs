//-----------------------------------------------------------------------
// <copyright file="CommandListenerClaims.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.IdentityModel.Claims;

    /// <summary>
    /// Represents the pre-defined claims that an entity can claim. 
    /// </summary>
    internal static class CommandListenerClaims
    {
        /// <summary>
        /// the claim that specifies a connection allowed for an entity.
        /// </summary>
        private static Claim connectionAllowedClaim = new Claim(EngineClaimTypes.ConnectionAllowed, true, Rights.PossessProperty);

        /// <summary>
        /// Gets the claim that specifies a connection allowed for an entity.
        /// </summary>
        public static Claim ConnectionAllowedClaim
        {
            get
            {
                return connectionAllowedClaim;
            }
        }
    }
}
