//-----------------------------------------------------------------------
// <copyright file="CommandActionBuilderDispatchFilterFactory.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// Implements a dispatch filter factory for create new action pipeline builder dispatch filters.
    /// </summary>
    internal sealed class CommandActionBuilderDispatchFilterFactory : DispatchFilterFactory<ICompileFilterContext, IEnumerable<ICommandActionExecutionContext>>
    {
        /// <inheritdoc />
        public override DispatchFilter<ICompileFilterContext, IEnumerable<ICommandActionExecutionContext>> Create()
        {
            return new ActionPipelineBuilderDispatchFilter();
        }
    }
}
