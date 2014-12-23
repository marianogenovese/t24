﻿//-----------------------------------------------------------------------
// <copyright file="CommandListenerServiceCredentials.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.IdentityModel.Selectors;
    using System.ServiceModel.Description;
    
    /// <summary>
    /// Allow to create a custom security token manager for the command listener.
    /// </summary>
    internal sealed class CommandListenerServiceCredentials : ServiceCredentials
    {
        /// <inheritdoc />
        public CommandListenerServiceCredentials() : base()
        {
        }

        /// <inheritdoc />
        public override SecurityTokenManager CreateSecurityTokenManager()
        {
            return new CommandListenerSecurityTokenManager(this);
        }

        /// <inheritdoc />
        protected override ServiceCredentials CloneCore()
        {
            return new CommandListenerServiceCredentials();
        }
    }
}
