//-----------------------------------------------------------------------
// <copyright file="TypeExtensions.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Diagnostics.Contracts;
    
    /// <summary>
    /// Provides types extensions methods used as utility functions
    /// </summary>
    internal static class TypeExtensions
    {
        /// <summary>
        /// Check is the type is a factory for dispatch filters.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if the type if a factory otherwise false.</returns>
        public static bool IsDispatchFilterFactory(this Type type)
        {
            if (type.BaseType != null)
            {
                if (type.BaseType.IsDispatchFilterFactory())
                {
                    return true;
                }
            }

            if (type.IsGenericType)
            {
                return type.GetGenericTypeDefinition() == typeof(DispatchFilterFactory<,>);
            }

            return false;
        }
        
        /// <summary>
        /// Check is the type is a factory for action filters.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if the type if a factory otherwise false.</returns>
        public static bool IsActionFilterFactory(this Type type)
        {
            if (type.BaseType != null)
            {
                if (type.BaseType.IsActionFilterFactory())
                {
                    return true;
                }
            }
            
            return type == typeof(CommandActionFactory);
        }
        
        /// <summary>
        /// Gets the arguments of the dispatch filter factory.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>An array of types.</returns>
        public static Type[] GetDispatchFilterFactoryArgs(this Type type)
        {
            if (type.BaseType != null)
            {
                if (type.BaseType.IsDispatchFilterFactory())
                {
                    return type.BaseType.GetGenericArguments();
                }
            }

            if (type.IsDispatchFilterFactory())
            {
                return type.GetGenericArguments();
            }
            
            return null;
        }
    }
}
