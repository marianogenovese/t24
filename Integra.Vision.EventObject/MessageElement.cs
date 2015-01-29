//-----------------------------------------------------------------------
// <copyright file="MessageElement.cs" company="Ingetra.Vision.Common">
//     Copyright (c) Ingetra.Vision.Common. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Event
{
    using System;

    /// <summary>
    /// MessageElement class
    /// </summary>
    /// <typeparam name="TElement">MessageElementNode class</typeparam>
    [Serializable]
    public abstract class MessageElement<TElement> : MessageElementCollection<TElement> where TElement : MessageElementNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageElement{TElement}"/> class
        /// </summary>
        /// <param name="identifier">Element identifier</param>
        /// <param name="name">Element name</param>
        public MessageElement(int identifier, string name) : base(identifier, name)
        {
        }
    }
}
