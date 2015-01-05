//-----------------------------------------------------------------------
// <copyright file="DispatchFilter{T}.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.Collections.Generic;
    
    /// <summary>
    /// Allow to implement filters or steps in a pipeline.
    /// </summary>
    /// <typeparam name="T">The type of the input and the type of the output.</typeparam>
    internal abstract class DispatchFilter<T> : DispatchFilter<T, T>
    {
    }
}
