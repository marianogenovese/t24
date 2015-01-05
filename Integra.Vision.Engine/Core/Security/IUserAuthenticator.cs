//-----------------------------------------------------------------------
// <copyright file="IUserAuthenticator.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// Specifies methods for user authentication.
    /// </summary>
    internal interface IUserAuthenticator
    {
        /// <summary>
        /// Check is a user is valid.
        /// </summary>
        /// <param name="user">The user name.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>True if the is valid; otherwise false.</returns>
        bool Validate(string user, string password);
    }
}
