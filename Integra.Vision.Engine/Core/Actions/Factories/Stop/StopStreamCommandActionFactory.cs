﻿//-----------------------------------------------------------------------
// <copyright file="StopStreamCommandActionFactory.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// Implements a action filter factory for create new action pipeline filters.
    /// This factory creates action filters for handle Stop Stream Command
    /// </summary>
    internal sealed class StopStreamCommandActionFactory : CommandActionFactory
    {
        /// <inheritdoc />
        public override CommandAction Create()
        {
            return new StopStreamCommandAction();
        }
    }
}
