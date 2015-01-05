//-----------------------------------------------------------------------
// <copyright file="CommandActionResult.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    
    /// <summary>
    /// Represent a result of a action execution.
    /// </summary>
    [DataContract(Name = "CommandResult", Namespace = "http://Integra.Vision.Engine/")]
    [KnownType("GetKnownTypes")]
    internal abstract class CommandActionResult
    {
        /// <summary>
        /// This method search in the current executing assembly all the command action results.
        /// </summary>
        /// <returns>The types.</returns>
        public static IEnumerable<Type> GetKnownTypes()
        {
            Type[] values = System.Reflection.Assembly.GetExecutingAssembly().GetTypes().Where(type => type.IsClass && !type.IsAbstract && typeof(CommandActionResult).IsAssignableFrom(type)).OrderBy(type => type.Name).ToArray();
            return values;
        }
    }
}
