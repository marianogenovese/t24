//-----------------------------------------------------------------------
// <copyright file="ResourceGate.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// This class implements the used of a resource lock and acts like a firewall
    /// requesting resource lock and creating new access tokens.
    /// </summary>
    internal class ResourceGate
    {
        /// <summary>
        /// The lock used for request access.
        /// </summary>
        private readonly ResourceLock @lock;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceGate"/> class.
        /// </summary>
        /// <param name="lock">The lock used for request access.</param>
        public ResourceGate(ResourceLock @lock)
        {
            this.@lock = @lock;
        }

        /// <summary>
        /// Makes a request for access to a protected resource.
        /// </summary>
        /// <param name="exclusive">true for exclusive access.</param>
        /// <returns>A access token that hold the lock.</returns>
        public ResourceAccessToken LockResource(bool exclusive)
        {
            return new ResourceAccessToken(this.@lock, exclusive);
        }
    }
}
