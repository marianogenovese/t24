//-----------------------------------------------------------------------
// <copyright file="DispatchFilterFactory{T}.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{    
    /// <summary>
    /// Allow to implement filters or steps in a pipeline.
    /// </summary>
    /// <typeparam name="T">Input type of the source filter</typeparam>
    internal abstract class DispatchFilterFactory<T> : DispatchFilterFactory<T, T>
    {
    }
}
