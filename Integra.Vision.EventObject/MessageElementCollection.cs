//-----------------------------------------------------------------------
// <copyright file="MessageElementCollection.cs" company="Ingetra.Vision.Common">
//     Copyright (c) Ingetra.Vision.Common. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Event
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Message element collection class
    /// </summary>
    /// <typeparam name="TElement">MessageElementNode class</typeparam>
    [Serializable]
    public class MessageElementCollection<TElement> : MessageElementNode, IEnumerable<TElement>, ICollection<TElement>, IEnumerable, IDisposable where TElement : MessageElementNode
    {
        /// <summary>
        /// List of elements
        /// </summary>
        private List<TElement> messageElementList = new List<TElement>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageElementCollection{TElement}"/> class
        /// </summary>
        /// <param name="identifier">Element identifier</param>
        /// <param name="name">Element name</param>
        public MessageElementCollection(int identifier, string name) : base(identifier, name)
        {
        }

        /// <inheritdoc />
        public int Count
        {
            get { return this.messageElementList.Count; }
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Get the message element by a specific identifier.
        /// </summary>
        /// <param name="identifier">Message element identifier</param>
        /// <returns>Message element</returns>
        public TElement this[int identifier]
        {
            get
            {
                return this.messageElementList.Find(x => x.Identifier == identifier);
            }
        }

        /// <summary>
        /// Get the message element by a specific name.
        /// </summary>
        /// <param name="name">Message element name</param>
        /// <returns>Message element</returns>
        public TElement this[string name]
        {
            get
            {
                return this.messageElementList.Find(x => x.Name == name);
            }
        }

        /// <inheritdoc />
        public IEnumerator<TElement> GetEnumerator()
        {
            return this.messageElementList.GetEnumerator();
        }

        /// <inheritdoc />
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.messageElementList).GetEnumerator();
        }

        /// <inheritdoc />
        public void Add(TElement item)
        {
            this.messageElementList.Add(item);
        }

        /// <inheritdoc />
        public void Clear()
        {
            this.messageElementList.Clear();
        }

        /// <inheritdoc />
        public bool Contains(TElement item)
        {
            return this.messageElementList.Contains(item);
        }

        /// <summary>
        /// Doc goes here
        /// </summary>
        /// <param name="sectionName">Name of the section</param>
        /// <returns>Whether the section exists or not</returns>
        public bool Contains(string sectionName)
        {
            return this.messageElementList.Exists(x => x.Name == sectionName);
        }

        /// <summary>
        /// Doc goes here
        /// </summary>
        /// <param name="sectionIdentifier">Identifier of the section</param>
        /// <returns>Whether the section exists or not</returns>
        public bool Contains(int sectionIdentifier)
        {
            return this.messageElementList.Exists(x => x.Identifier == sectionIdentifier);
        }

        /// <inheritdoc />
        public void CopyTo(TElement[] array, int arrayIndex)
        {
            this.messageElementList.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public bool Remove(TElement item)
        {
            return this.messageElementList.Remove(item);
        }
    }
}
