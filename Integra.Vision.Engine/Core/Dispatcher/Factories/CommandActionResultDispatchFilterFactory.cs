//-----------------------------------------------------------------------
// <copyright file="CommandActionResultDispatchFilterFactory.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// Implements a dispatch filter factory for create new action pipeline invoker dispatch filters.
    /// </summary>
    internal sealed class CommandActionResultDispatchFilterFactory : DispatchFilterFactory<IEnumerable<ICommandActionExecutionContext>, OperationContext>
    {
        /// <inheritdoc />
        public override DispatchFilter<IEnumerable<ICommandActionExecutionContext>, OperationContext> Create()
        {
            return new ActionResultPipelineDispatchFilter();
        }
    }
}
