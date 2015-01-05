//-----------------------------------------------------------------------
// <copyright file="ReadOnlyNamedElementCollectionBase.cs" company="CompanyName">
//     Copyright (c) CompanyName. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Integra.Vision;

    /// <summary>
    /// ReadOnlyNamedElementCollectionBase
    /// Doc goes here
    /// </summary>
    /// <typeparam name="T">generic doc goes here</typeparam>
    internal abstract class ReadOnlyNamedElementCollectionBase<T> : IReadOnlyNamedElementCollection<T> where T : INamedElement
    {
        /// <summary>
        /// Doc goes here
        /// </summary>
        private ReadOnlyDictionary<string, T> readOnlyDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyNamedElementCollectionBase{T}"/> class
        /// </summary>
        /// <param name="elements">elements doc goes here</param>
        public ReadOnlyNamedElementCollectionBase(T[] elements)
        {
            Dictionary<string, T> elementsDictionary = new Dictionary<string, T>();
            foreach (T element in elements)
            {
                elementsDictionary.Add(element.Name, element);
            }

            this.readOnlyDictionary = new ReadOnlyDictionary<string, T>(elementsDictionary);
        }

        /// <summary>
        /// Gets
        /// Doc goes here
        /// </summary>
        public int Count
        {
            get
            {
                return this.readOnlyDictionary.Count;
            }
        }

        /// <summary>
        /// Gets
        /// Doc goes here
        /// </summary>
        /// <param name="name">name doc goes here</param>
        /// <returns>Doc goes here</returns>
        public T this[string name]
        {
            get
            {
                return this.readOnlyDictionary[name];
            }
        }

        /// <summary>
        /// Gets
        /// Doc goes here
        /// </summary>
        /// <returns>Doc goes here</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.readOnlyDictionary.Values.GetEnumerator();
        }

        /// <summary>
        /// Gets
        /// Doc goes here
        /// </summary>
        /// <returns>Doc goes here</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.readOnlyDictionary.Values.GetEnumerator();
        }
    }
}
