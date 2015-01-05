//-----------------------------------------------------------------------
// <copyright file="ReusableBuffer.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.IO;
    
    /// <summary>
    /// Represents a buffer with data which after use will be returned to buffer manager.
    /// </summary>
    internal sealed class ReusableBuffer : IDisposable
    {        
        /// <summary>
        /// The data of the buffer.
        /// </summary>
        private readonly byte[] data;

        /// <summary>
        /// Used for return the buffer.
        /// </summary>
        private readonly IBufferManager bufferManager;

        /// <summary>
        /// The length of the buffer.
        /// </summary>
        private readonly long length;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ReusableBuffer"/> class.
        /// </summary>
        /// <param name="data">The data of the buffer.</param>
        /// <param name="length">The length of the data in the buffer.</param>
        /// <param name="bufferManager">Used for return the buffer.</param>
        public ReusableBuffer(byte[] data, long length, IBufferManager bufferManager)
        {
            this.data = data;
            this.length = length;
            this.bufferManager = bufferManager;
        }

        /// <summary>
        /// Gets the length of the buffer.
        /// </summary>
        public long Length
        {
            get
            {
                return this.length;
            }
        }

        /// <summary>
        /// Gets the data of the buffer.
        /// </summary>
        public byte[] Data
        {
            get
            {
                return this.data;
            }
        }
        
        /// <summary>
        /// Read all bytes in the stream and return a buffer.
        /// </summary>
        /// <param name="stream">The stream which contains the data to read.</param>
        /// <returns>A reusable buffer containing the data that was read.</returns>
        public static ReusableBuffer ReadAll(Stream stream)
        {
            IBufferManager bufferManager = DependencyResolver.Default.Resolve<IBufferManager>();
            byte[] buffer = bufferManager.Take((int)stream.Length);
            stream.Read(buffer, 0, buffer.Length);
            return new ReusableBuffer(buffer, stream.Length, bufferManager);
        }

        /// <summary>
        /// Return the buffer to the buffer manager.
        /// </summary>
        public void Dispose()
        {
            this.bufferManager.Return(this.data);
        }
    }
}
