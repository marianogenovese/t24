//-----------------------------------------------------------------------
// <copyright file="AuthorizationCommandAction.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.Security.Claims;
    
    /// <summary>
    /// Implements the action of check if the user has authorization for execute the command.
    /// </summary>
    internal sealed class AuthorizationCommandAction : AuthorizationCommandActionBase
    {
        /// <inheritdoc />
        protected override bool IsAuthorized(AuthorizationContext context)
        {
            ClaimsPrincipal principal = context.User as ClaimsPrincipal;

            return true;
        }
    }
}
