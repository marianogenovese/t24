//-----------------------------------------------------------------------
// <copyright file="ReadOnlyBufferPoolStream.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.IO;

    /// <summary>
    /// Implements a in-memory stream that uses the buffer manager to take buffers and when the stream is disposed
    /// the buffers used in the stream are returned to the manager for reuse. This is the read-only version.
    /// </summary>
    internal sealed class ReadOnlyBufferPoolStream : Stream
    {
        /// <summary>
        /// Contains the response data.
        /// </summary>
        private readonly BufferPoolStream stream;

        /// <summary>
        /// For reuse buffers.
        /// </summary>
        private readonly IBufferManager bufferManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBufferPoolStream"/> class.
        /// </summary>
        /// <param name="stream">Contains the response data.</param>
        /// <param name="bufferManager">For reuse buffers.</param>
        public ReadOnlyBufferPoolStream(BufferPoolStream stream, IBufferManager bufferManager)
        {
            this.stream = stream;
            this.bufferManager = bufferManager;
        }
        
        /// <inheritdoc />
        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        /// <inheritdoc />
        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        /// <inheritdoc />
        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        /// <inheritdoc />
        public override long Length
        {
            get
            {
                return this.stream.Length;
            }
        }

        /// <inheritdoc />
        public override long Position
        {
            get
            {
                return this.stream.Position;
            }
            
            set
            {
                this.stream.Position = value;
            }
        }

        /// <inheritdoc />
        public override void Flush()
        {
        }

        /// <inheritdoc />
        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override void SetLength(long value)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new System.NotImplementedException();
        }
    }
}
