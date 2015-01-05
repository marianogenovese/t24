//-----------------------------------------------------------------------
// <copyright file="DbUserAuthenticator.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// Implements a user authenticator which use the register users in a database.
    /// </summary>
    internal sealed class DbUserAuthenticator : IUserAuthenticator
    {
        /// <inheritdoc />
        public bool Validate(string user, string password)
        {
            if (string.Equals(user, "mariano", System.StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            
            /*Hacer la lógica para ir a la db.*/
            return false;
        }
    }
}
