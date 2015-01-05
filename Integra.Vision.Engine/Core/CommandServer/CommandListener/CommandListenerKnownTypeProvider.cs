//-----------------------------------------------------------------------
// <copyright file="CommandListenerKnownTypeProvider.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    
    /// <summary>
    /// Command Listener type provider
    /// </summary>
    internal static class CommandListenerKnownTypeProvider
    {
        /// <summary>
        /// Finds all the command action results.
        /// </summary>
        /// <param name="provider">A attribute provider.</param>
        /// <returns>The types.</returns>
        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
        {
            Type[] values = System.Reflection.Assembly.GetExecutingAssembly().GetTypes().Where(type => type.IsClass && !type.IsAbstract && typeof(CommandActionResult).IsAssignableFrom(type)).OrderBy(type => type.Name).ToArray();
            return values;
        }
    }
}
