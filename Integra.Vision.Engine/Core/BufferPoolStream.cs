//-----------------------------------------------------------------------
// <copyright file="BufferPoolStream.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.ServiceModel.Channels;
    
    /// <summary>
    /// Implements a in-memory stream that uses the buffer manager to take buffers and when the stream is disposed
    /// the buffers used in the stream are returned to the manager for reuse.
    /// </summary>
    internal sealed class BufferPoolStream : Stream, IEnumerable<byte[]>
    {
        /// <summary>
        /// The manager used for take and release buffers.
        /// </summary>
        private readonly IBufferManager bufferManager;
        
        /// <summary>
        /// The list of chunks used in the streams
        /// </summary>
        private readonly List<byte[]> chunks;
        
        /// <summary>
        /// The size of the chunk to take when the stream grow.
        /// </summary>
        private int chunkSize;
        
        /// <summary>
        /// The current position of the stream.
        /// </summary>
        private long currentPosition = 0;
        
        /// <summary>
        /// The length of the stream.
        /// </summary>
        private long length;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="BufferPoolStream"/> class.
        /// </summary>
        /// <param name="bufferManager">The manager used to take or release buffers.</param>
        /// <param name="chunkSize">The size of the buffers to take.</param>
        public BufferPoolStream(IBufferManager bufferManager, int chunkSize)
        {
            this.bufferManager = bufferManager;
            this.chunkSize = chunkSize;
            this.chunks = new List<byte[]>();
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
                return true;
            }
        }

        /// <inheritdoc />
        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }
        
        /// <summary>
        /// Gets the current capacity of the stream.
        /// </summary>
        public long Capacity
        {
            get
            {
                return this.chunks.Count * this.chunkSize;
            }
        }
        
        /// <inheritdoc />
        public override long Length
        {
            get
            {
                return this.length;
            }
        }
        
        /// <inheritdoc />
        public override long Position
        {
            get
            {
                return this.currentPosition;
            }
            
            set
            {
                if (value < 0 || value > this.Length)
                {
                    throw new ArgumentOutOfRangeException();
                }

                this.currentPosition = value;
            }
        }

        /// <summary>
        /// Creates a instance of a <see cref="BufferPoolStream"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="BufferPoolStream"/></returns>
        /// <param name="chunkSize">The size of the buffers to take.</param>
        public static BufferPoolStream Create(int chunkSize)
        {
            return new BufferPoolStream(DependencyResolver.Default.Resolve<IBufferManager>(), chunkSize);
        }
        
        /// <inheritdoc />
        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    this.SeekFromBegining(offset);
                    break;

                case SeekOrigin.Current:
                    this.SeekFromCurrent(offset);
                    break;

                case SeekOrigin.End:
                    this.SeekFromEnd(offset);
                    break;

                default:
                    throw new ArgumentOutOfRangeException("origin");
            }

            return this.Position;
        }
        
        /// <inheritdoc />
        public override void SetLength(long newLength)
        {
            if (newLength < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (newLength > this.Capacity)
            {
                this.EnsureCapacity(newLength);
            }
            else if (newLength < this.Length)
            {
                this.ShrinkCapacity(newLength);
            }

            this.length = newLength;
            this.Position = Math.Min(this.Position, this.length);
        }
        
        /// <inheritdoc />
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            if (offset + count > buffer.Length)
            {
                throw new ArgumentException("offset");
            }

            int bytesToRead = count;
            int bytesReaded = 0;

            while (bytesToRead > 0)
            {
                if (this.currentPosition >= this.length)
                {
                    return bytesReaded;
                }

                int currentChunk = this.GetCurrentChunkIndex();
                int currentChunkPosition = this.GetPositionInCurrentChunk();
                long bytesToStreamEnd = this.length - this.currentPosition;
                long bytesToChunkEnd = this.chunkSize - currentChunkPosition;
                int bytesToCopy = (int)Utils.Min(bytesToStreamEnd, bytesToChunkEnd, bytesToRead);
                Buffer.BlockCopy(this.chunks[currentChunk], currentChunkPosition, buffer, offset + bytesReaded, bytesToCopy);
                bytesToRead -= bytesToCopy;
                bytesReaded += bytesToCopy;
                this.currentPosition += bytesToCopy;
            }

            return bytesReaded;
        }
        
        /// <inheritdoc />
        public override int ReadByte()
        {
            if (this.currentPosition >= this.length)
            {
                return -1;
            }

            int currentChunk = this.GetCurrentChunkIndex();
            int currentChunkPosition = this.GetPositionInCurrentChunk();
            int @byte = this.chunks[currentChunk][currentChunkPosition];
            this.currentPosition++;
            return @byte;
        }

        /// <summary>
        /// Writes all data from array to stream.
        /// </summary>
        /// <param name="buffer">The array of bytes to write.</param>
        public void Write(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            this.Write(buffer, 0, buffer.Length);
        }
        
        /// <inheritdoc />
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            if (offset + count > buffer.Length)
            {
                throw new ArgumentOutOfRangeException("offset");
            }

            long requiredCapacity = this.currentPosition + count;
            this.EnsureCapacity(requiredCapacity);

            int bytesToWrite = count;
            int bytesWritten = 0;
            while (bytesToWrite > 0)
            {
                int currentChunk = this.GetCurrentChunkIndex();
                int currentChunkPosition = this.GetPositionInCurrentChunk();
                int bytesToChunkEnd = (int)(this.chunkSize - currentChunkPosition);
                int bytesToWriteInCurrentChunk = Math.Min(bytesToChunkEnd, bytesToWrite);
                Buffer.BlockCopy(buffer, offset + bytesWritten, this.chunks[currentChunk], currentChunkPosition, bytesToWriteInCurrentChunk);
                this.currentPosition += bytesToWriteInCurrentChunk;
                bytesWritten += bytesToWriteInCurrentChunk;
                bytesToWrite -= bytesToWriteInCurrentChunk;
            }

            this.length = Math.Max(this.length, requiredCapacity);
        }
        
        /// <inheritdoc />
        public override void WriteByte(byte value)
        {
            this.EnsureCapacity(this.currentPosition + 1);
            int currentChunk = this.GetCurrentChunkIndex();
            int currentChunkPosition = this.GetPositionInCurrentChunk();
            this.chunks[currentChunk][currentChunkPosition] = value;
            this.currentPosition++;
            this.length = Math.Max(this.length, this.currentPosition);
        }
        
        /// <summary>
        /// Clear the list of the chunks.
        /// </summary>
        public void Clear()
        {
            this.chunks.Clear();
            this.currentPosition = this.length = 0;
        }
        
        /// <inheritdoc />
        public override void Flush()
        {
        }

        /// <inheritdoc />
        public IEnumerator<byte[]> GetEnumerator()
        {
            return this.chunks.GetEnumerator();
        }

        /// <inheritdoc />
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.chunks.GetEnumerator();
        }
        
        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            this.ReturnChunks();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Creates a new chunk using the buffer manager.
        /// </summary>
        /// <returns>The new chunk.</returns>
        private byte[] CreateChunk()
        {
            return this.bufferManager.Take(this.chunkSize);
        }

        /// <summary>
        /// Seek in the stream from the end.
        /// </summary>
        /// <param name="offset">The offset to seek from.</param>
        private void SeekFromEnd(long offset)
        {
            offset = this.Length + offset;
            this.SeekFromBegining(offset);
        }

        /// <summary>
        /// Seek in the stream from the current position.
        /// </summary>
        /// <param name="offset">The offset to seek from.</param>
        private void SeekFromCurrent(long offset)
        {
            offset = offset + this.Position;
            this.SeekFromBegining(offset);
        }
        
        /// <summary>
        /// Seek in the stream from the begin.
        /// </summary>
        /// <param name="offset">The offset to seek from.</param>
        private void SeekFromBegining(long offset)
        {
            this.Position = offset.Clamp(0, this.Length);
        }

        /// <summary>
        /// Shrink the stream to a new length.
        /// </summary>
        /// <param name="newLength">The length to shrink.</param>
        private void ShrinkCapacity(long newLength)
        {
            long chunksNeeded = (newLength % this.chunkSize == 0) ? (newLength / this.chunkSize) : (newLength / this.chunkSize) + 1;
            int cn = (int)chunksNeeded;
            this.chunks.RemoveRange(cn, this.chunks.Count - cn);
        }

        /// <summary>
        /// If the requested capacity exceeds the current capacity creates new chunks to ensure the proper capacity of the stream.
        /// </summary>
        /// <param name="requestedCapacity">The capacity requested.</param>
        private void EnsureCapacity(long requestedCapacity)
        {
            while (requestedCapacity > this.Capacity)
            {
                this.chunks.Add(this.CreateChunk());
            }
        }
        
        /// <summary>
        /// Gets the index of the current chunk.
        /// </summary>
        /// <returns>The index of the current chunk.</returns>
        private int GetCurrentChunkIndex()
        {
            return (int)(this.currentPosition / this.chunkSize);
        }

        /// <summary>
        /// Calculate the position of the current chunk.
        /// </summary>
        /// <returns>The position in the current chunk.</returns>
        private int GetPositionInCurrentChunk()
        {
            return (int)(this.currentPosition % this.chunkSize);
        }

        /// <summary>
        /// Return the chunks used to the manager.
        /// </summary>
        private void ReturnChunks()
        {
            // If the buffer pool always allocates a new buffer,
            //   there's no point to trying to return all of the buffers.
            if (this.chunks != null)
            {
                foreach (byte[] chunk in this.chunks)
                {
                    this.bufferManager.Return(chunk);
                }
                
                this.Clear();
            }
        }
    }
}
