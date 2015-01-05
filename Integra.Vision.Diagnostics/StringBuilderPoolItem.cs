//-----------------------------------------------------------------------
// <copyright file="StringBuilderPoolItem.cs" company="Integra.Vision.Diagnostics">
//     Copyright (c) Integra.Vision.Diagnostics. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Diagnostics
{
    using System;
    using System.Text;
    
    /// <summary>
    /// Represents a item which is used in the pool of String Builders.
    /// </summary>
    internal sealed class StringBuilderPoolItem : IDisposable
    {
        /// <summary>
        /// The String Builder instance.
        /// </summary>
        private StringBuilder stringBuilder = new StringBuilder();

        /// <summary>
        /// Gets the String Builder instance.
        /// </summary>
        public StringBuilder StringBuilder
        {
            get
            {
                return this.stringBuilder;
            }
        }

        /// <summary>
        /// Free the resources of the related String Builder instance.
        /// </summary>
        public void Clear()
        {
            this.stringBuilder.Clear();
        }

        /// <summary>
        /// Return this item to the String Builder pool for reuse.
        /// </summary>
        public void Dispose()
        {
            StringBuilderPool.Return(this);
        }
    }
}
