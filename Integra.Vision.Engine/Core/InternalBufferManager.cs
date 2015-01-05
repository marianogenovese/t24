//-----------------------------------------------------------------------
// <copyright file="InternalBufferManager.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.ServiceModel.Channels;
    
    /// <summary>
    /// Internal implementation of the buffer management using Service Model Buffer Manager class.
    /// </summary>
    internal sealed class InternalBufferManager : IBufferManager
    {
        /// <summary>
        /// The buffer manager.
        /// </summary>
        private readonly BufferManager bufferManager;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="InternalBufferManager"/> class.
        /// </summary>
        /// <param name="maxBufferPoolSize">The maximum size of the pool.</param>
        /// <param name="maxBufferSize">The maximum size of an individual buffer.</param>
        public InternalBufferManager(long maxBufferPoolSize, int maxBufferSize)
        {
            this.bufferManager = BufferManager.CreateBufferManager(maxBufferPoolSize, maxBufferSize);
        }

        /// <inheritdoc />
        public byte[] Take(int bufferSize)
        {
            return this.bufferManager.TakeBuffer(bufferSize);
        }

        /// <inheritdoc />
        public void Return(byte[] buffer)
        {
            this.bufferManager.ReturnBuffer(buffer);
        }
    }
}
