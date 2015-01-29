//-----------------------------------------------------------------------
// <copyright file="ReceiveCommandActionFactory.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// Implements a action filter factory for create new action pipeline filters.
    /// This factory creates action filters for handle Receive Command
    /// </summary>
    internal sealed class ReceiveCommandActionFactory : CommandActionFactory
    {
        /// <inheritdoc />
        public override CommandAction Create()
        {
            return new ReceiveCommandAction();
        }
    }
}
