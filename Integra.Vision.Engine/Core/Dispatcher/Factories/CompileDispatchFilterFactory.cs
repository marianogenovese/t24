//-----------------------------------------------------------------------
// <copyright file="CompileDispatchFilterFactory.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// Implements a dispatch filter factory for create new compile dispatch filters.
    /// </summary>
    internal sealed class CompileDispatchFilterFactory : DispatchFilterFactory<IParseFilterContext, ICompileFilterContext>
    {
        /// <inheritdoc />
        public override DispatchFilter<IParseFilterContext, ICompileFilterContext> Create()
        {
            return new CompileDispatchFilter();
        }
    }
}
