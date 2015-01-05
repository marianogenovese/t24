//-----------------------------------------------------------------------
// <copyright file="IReadOnlyNamedElementCollection.cs" company="Integra.Vision">
//     Copyright (c) Integra.Vision. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision
{
    // TODO Doc
    using System.Collections.Generic;    
    
    /// <summary>
    /// IReadOnlyNamedElementCollection
    /// Doc goes here
    /// </summary>
    /// <typeparam name="T">T doc goes here</typeparam>
    internal interface IReadOnlyNamedElementCollection<out T> : IReadOnlyCollection<T> where T : INamedElement
    {
        /// <summary>
        /// Gets
        /// Doc goes here
        /// </summary>
        /// <param name="name">name doc goes here</param>
        /// <returns>Doc goes here</returns>
        T this[string name] { get; }
    }
}
