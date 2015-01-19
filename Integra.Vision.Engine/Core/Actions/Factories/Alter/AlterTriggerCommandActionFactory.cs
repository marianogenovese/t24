﻿//-----------------------------------------------------------------------
// <copyright file="AlterTriggerCommandActionFactory.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// Implements a action filter factory for create new action pipeline filters.
    /// This factory creates action filters for handle Alter Trigger Command
    /// </summary>
    internal sealed class AlterTriggerCommandActionFactory : CommandActionFactory
    {
        /// <inheritdoc />
        public override CommandAction Create()
        {
            return new AlterTriggerCommandAction();
        }
    }
}