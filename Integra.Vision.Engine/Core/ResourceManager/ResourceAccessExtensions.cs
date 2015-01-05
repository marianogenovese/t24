//-----------------------------------------------------------------------
// <copyright file="ResourceAccessExtensions.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// Provides resource access extensions methods used as utility functions.
    /// </summary>
    internal static class ResourceAccessExtensions
    {
        /// <summary>
        /// create a new access used exclusively for writing and is linked to another access resources.
        /// </summary>
        /// <typeparam name="T">the type of resource that has been requested access.</typeparam>
        /// <param name="resourceAccess">The related resource access.</param>
        /// <returns>A new linked resource access.</returns>
        public static ResourceAccess LinkWriteAccess<T>(this ResourceAccess resourceAccess)
        {
            ResourceAccess<T> newResourceAccess = ResourceManager.WriteAccess<T>();
            return new NestedResourceAccessWrapper<T>(newResourceAccess, resourceAccess);
        }

        /// <summary>
        /// create a new access used exclusively for reading and is linked to another access resources
        /// </summary>
        /// <typeparam name="T">the type of resource that has been requested access.</typeparam>
        /// <param name="resourceAccess">The related resource access.</param>
        /// <returns>A new linked resource access.</returns>
        public static ResourceAccess ReadAccess<T>(this ResourceAccess resourceAccess)
        {
            ResourceAccess<T> newResourceAccess = ResourceManager.ReadAccess<T>();
            return new NestedResourceAccessWrapper<T>(newResourceAccess, resourceAccess);
        }
    }
}