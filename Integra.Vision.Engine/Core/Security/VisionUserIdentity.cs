//-----------------------------------------------------------------------
// <copyright file="VisionUserIdentity.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.Security.Claims;
    using System.Security.Principal;

    /// <summary>
    /// Represents a user that is defined in the engine.
    /// </summary>
    internal sealed class VisionUserIdentity : ClaimsIdentity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisionUserIdentity"/> class.
        /// </summary>
        /// <param name="user">The identity from which to base the new claims identity.</param>
        /// <param name="claims">The claims with which to populate the claims identity.</param>
        public VisionUserIdentity(string user, Claim[] claims) : base(new GenericIdentity(user), claims)
        {
        }

        /// <inheritdoc />
        public override bool IsAuthenticated
        {
            get
            {
                return true;
            }
        }
    }
}
