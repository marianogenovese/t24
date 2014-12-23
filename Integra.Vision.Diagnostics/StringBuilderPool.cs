//-----------------------------------------------------------------------
// <copyright file="StringBuilderPool.cs" company="Integra.Vision.Diagnostics">
//     Copyright (c) Integra.Vision.Diagnostics. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Diagnostics
{
    using System.Collections.Concurrent;

    /// <summary>
    /// This class create a pool of StringBuilder instances available for use and reuse.
    /// </summary>
    internal static class StringBuilderPool
    {
        /// <summary>
        /// Maximum number of instances available.
        /// </summary>
        private const int MaxPooledStringBuilders = 128;

        /// <summary>
        /// The pool of string builders.
        /// </summary>
        private static readonly ConcurrentQueue<StringBuilderPoolItem> FreeStringBuilders = new ConcurrentQueue<StringBuilderPoolItem>();

        /// <summary>
        /// Take an instance of an String Builder available.
        /// </summary>
        /// <returns>The String Builder available.</returns>
        public static StringBuilderPoolItem Take()
        {
            StringBuilderPoolItem sb = null;
            if (FreeStringBuilders.TryDequeue(out sb))
            {
                return sb;
            }

            return new StringBuilderPoolItem();
        }
        
        /// <summary>
        /// Return to the pool a instance of String Builder for reuse.
        /// </summary>
        /// <param name="item">The String Builder instance.</param>
        public static void Return(StringBuilderPoolItem item)
        {
            if (FreeStringBuilders.Count <= MaxPooledStringBuilders)
            {
                // There is a race condition here so the count could be off a little bit (but insignificantly)
                item.Clear();
                FreeStringBuilders.Enqueue(item);
            }
        }
    }
}
