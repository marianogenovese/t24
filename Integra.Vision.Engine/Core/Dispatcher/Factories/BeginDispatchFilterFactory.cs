//-----------------------------------------------------------------------
// <copyright file="BeginDispatchFilterFactory.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// Implements a dispatch filter factory for create new begin dispatch filters.
    /// </summary>
    internal sealed class BeginDispatchFilterFactory : DispatchFilterFactory<OperationContext, DispatchContext>
    {
        /// <inheritdoc />
        public override DispatchFilter<OperationContext, DispatchContext> Create()
        {
            return new BeginDispatchFilter();
        }
    }
}
