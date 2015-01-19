﻿//-----------------------------------------------------------------------
// <copyright file="SetTraceStreamCommandActionFactory.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// Implements a action filter factory for create new action pipeline filters.
    /// This factory creates action filters for handle Set Trace Stream Command
    /// </summary>
    internal sealed class SetTraceStreamCommandActionFactory : CommandActionFactory
    {
        /// <inheritdoc />
        public override CommandAction Create()
        {
            return new SetTraceStreamCommandAction();
        }
    }
}