﻿//-----------------------------------------------------------------------
// <copyright file="CommandActionInvokerDispatchFilterFactory.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// Implements a dispatch filter factory for create new action pipeline invoker dispatch filters.
    /// </summary>
    internal sealed class CommandActionInvokerDispatchFilterFactory : DispatchFilterFactory<IEnumerable<ICommandActionExecutionContext>, IEnumerable<ICommandActionExecutionContext>>
    {
        /// <inheritdoc />
        public override DispatchFilter<IEnumerable<ICommandActionExecutionContext>, IEnumerable<ICommandActionExecutionContext>> Create()
        {
            return new ActionPipelineInvokerDispatchFilter();
        }
    }
}