//-----------------------------------------------------------------------
// <copyright file="MessageSubsection.cs" company="Ingetra.Vision.Common">
//     Copyright (c) Ingetra.Vision.Common. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Event
{
    using System;

    /// <summary>
    /// Message subsection class
    /// </summary>
    [Serializable]
    public class MessageSubsection : MessageElement<MessageSubsection>
    {
        /// <summary>
        /// Element value
        /// </summary>
        private object value;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSubsection"/> class
        /// </summary>
        /// <param name="identifier">Element identifier</param>
        /// <param name="name">Element name</param>
        public MessageSubsection(int identifier, string name)
            : base(identifier, name)
        {
        }

        /// <summary>
        /// Gets or sets the value of the element
        /// </summary>
        public object Value
        {
            get
            {
                return this.value;
            }

            set
            {
                if (this.value == null)
                {
                    this.value = value;
                }
            }
        }
    }
}
