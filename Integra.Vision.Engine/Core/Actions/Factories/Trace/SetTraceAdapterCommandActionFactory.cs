//-----------------------------------------------------------------------
// <copyright file="SetTraceAdapterCommandActionFactory.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// Implements a action filter factory for create new action pipeline filters.
    /// This factory creates action filters for handle Set Trace Adapter Command
    /// </summary>
    internal sealed class SetTraceAdapterCommandActionFactory : CommandActionFactory
    {
        /// <inheritdoc />
        public override CommandAction Create()
        {
            return new SetTraceAdapterCommandAction();
        }
    }
}
