//-----------------------------------------------------------------------
// <copyright file="AuthorizationCommandActionBase.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// Implements the action of check if the user has authorization for execute the command.
    /// </summary>
    internal abstract class AuthorizationCommandActionBase : CommandAction
    {
        /// <summary>
        /// Check is the user has authorization.
        /// </summary>
        /// <param name="context">The action execution context.</param>
        protected override void OnExecute(CommandExecutingContext context)
        {
            if (!this.IsAuthorized(new AuthorizationContext(context)))
            {
                context.Result = new NotAuthorizedCommandResult();
            }
        }

        /// <summary>
        /// Determines whether access for this particular request is authorized.
        /// </summary>
        /// <param name="context">The authorization context.</param>
        /// <returns>true if access is authorized; otherwise false.</returns>
        protected abstract bool IsAuthorized(AuthorizationContext context);
    }
}
