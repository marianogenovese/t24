//-----------------------------------------------------------------------
// <copyright file="VisionClientResponseMessage.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.ServiceModel.Channels;
    using System.Xml;
    
    /// <summary>
    /// Implements the Integra Vision Client response message.
    /// </summary>
    internal class VisionClientResponseMessage : Message
    {
        /// <summary>
        /// The response data.
        /// </summary>
        private readonly Stream stream;

        /// <summary>
        /// The properties of the message.
        /// </summary>
        private MessageProperties properties;

        /// <summary>
        /// The headers of the message.
        /// </summary>
        private MessageHeaders headers;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="VisionClientResponseMessage"/> class.
        /// </summary>
        /// <param name="stream">The response data.</param>
        public VisionClientResponseMessage(Stream stream)
        {
            this.stream = stream;
            this.Headers.Action = Resources.SR.ExecuteReplyAction;
        }
        
        /// <inheritdoc />
        public override MessageHeaders Headers
        {
            get
            {
                if (this.headers == null)
                {
                    this.headers = new MessageHeaders(this.Version);
                }

                return this.headers;
            }
        }

        /// <inheritdoc />
        public override MessageProperties Properties
        {
            get
            {
                if (this.properties == null)
                {
                    this.properties = new MessageProperties();
                }

                return this.properties;
            }
        }
        
        /// <inheritdoc />
        public override MessageVersion Version
        {
            get
            {
                return MessageVersion.Default;
            }
        }

        /// <inheritdoc />
        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            using (ReusableBuffer buffer = ReusableBuffer.ReadAll(this.stream))
            {
                writer.WriteBinHex(buffer.Data, 0, (int)buffer.Length);
            }
        }

        /// <inheritdoc />
        protected override void OnClose()
        {
            using (this.stream)
            {
                base.OnClose();
            }
        }
    }
}
