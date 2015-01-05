//-----------------------------------------------------------------------
// <copyright file="IBufferManager.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// Specifies methods for the management of memory buffers. 
    /// </summary>
    internal interface IBufferManager
    {
        /// <summary>
        /// Gets a buffer of at least the specified size from the pool.
        /// </summary>
        /// <param name="bufferSize">The size, in bytes, of the requested buffer.</param>
        /// <returns>A byte array that is the requested size of the buffer.</returns>
        byte[] Take(int bufferSize);
        
        /// <summary>
        /// Returns a buffer to the pool.
        /// </summary>
        /// <param name="buffer">The buffer being returned.</param>
        void Return(byte[] buffer);
    }
}
