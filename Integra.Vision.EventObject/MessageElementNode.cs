//-----------------------------------------------------------------------
// <copyright file="MessageElementNode.cs" company="Ingetra.Vision.Common">
//     Copyright (c) Ingetra.Vision.Common. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Event
{
    using System;

    /// <summary>
    /// Message element node class
    /// </summary>
    [Serializable]
    public class MessageElementNode : IDisposable
    {
        /// <summary>
        /// Element identifier
        /// </summary>
        private int identifier;

        /// <summary>
        /// Element name
        /// </summary>
        private string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageElementNode"/> class
        /// </summary>
        /// <param name="identifier">Element identifier</param>
        /// <param name="name">Element name</param>
        public MessageElementNode(int identifier, string name)
        {
            this.identifier = identifier;
            this.name = name;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="MessageElementNode" /> class
        /// </summary>
        ~MessageElementNode()
        {
            // Finalizer calls Dispose(false)
            this.Dispose(false);
        }

        /// <summary>
        /// Gets the identifier of the element
        /// </summary>
        public int Identifier
        {
            get
            {
                return this.identifier;
            }
        }

        /// <summary>
        /// Gets the name of the element
        /// </summary>
        public string Name 
        {
            get
            {
                return this.name;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Doc goes here
        /// </summary>
        /// <param name="disposing">Indicates whether free managed resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
            }

            // free native resources if there are any.
            this.identifier = -1;

            if (this.name != null)
            {
                this.name = null;
            }
        }
    }
}
