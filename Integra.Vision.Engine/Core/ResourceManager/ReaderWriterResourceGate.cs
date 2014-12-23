//-----------------------------------------------------------------------
// <copyright file="ReaderWriterResourceGate.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// Implements a resource gate that use ReaderWriterLock.
    /// </summary>
    internal sealed class ReaderWriterResourceGate : ResourceGate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReaderWriterResourceGate"/> class.
        /// </summary>
        public ReaderWriterResourceGate() : base(new ReaderWriterResourceLock())
        {
        }
    }
}
