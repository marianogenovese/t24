//-----------------------------------------------------------------------
// <copyright file="MessageSection.cs" company="Ingetra.Vision.Common">
//     Copyright (c) Ingetra.Vision.Common. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Event
{
    using System;

    /// <summary>
    /// Message section class
    /// </summary>
    [Serializable]
    public class MessageSection : MessageElement<MessageSubsection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSection"/> class
        /// </summary>
        /// <param name="identifier">Element identifier</param>
        /// <param name="name">Element name</param>
        public MessageSection(int identifier, string name)
            : base(identifier, name)
        {
        }
    }
}
