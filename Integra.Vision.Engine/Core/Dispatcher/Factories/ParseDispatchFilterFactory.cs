//-----------------------------------------------------------------------
// <copyright file="ParseDispatchFilterFactory.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.Collections.Generic;
    
    /// <summary>
    /// Implements a dispatch filter factory for create new parse dispatch filters.
    /// </summary>
    internal sealed class ParseDispatchFilterFactory : DispatchFilterFactory<DispatchContext, IParseFilterContext>
    {
        /// <inheritdoc />
        public override DispatchFilter<DispatchContext, IParseFilterContext> Create()
        {
            return new ParseDispatchFilter();
        }
    }
}
