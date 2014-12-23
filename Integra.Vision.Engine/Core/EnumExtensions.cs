//-----------------------------------------------------------------------
// <copyright file="EnumExtensions.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;
    
    /// <summary>
    /// Provides enumerator extensions methods used as utility functions
    /// </summary>
    internal static class EnumExtensions
    {
        /// <summary>
        /// Get the string representation of the command type.
        /// </summary>
        /// <param name="commandType">The command type</param>
        /// <returns>A string representation of the command type.</returns>
        public static string GetStringRepresentation(this CommandTypeEnum commandType)
        {
            return commandType.ToString("g");
        }
        
        /// <summary>
        /// Try to parse a string to a command type enumeration
        /// </summary>
        /// <param name="commandTypeAsString">The command type as string</param>
        /// <returns>A conversion of string to command type enumeration.</returns>
        public static CommandTypeEnum AsCommandTypeEnum(this string commandTypeAsString)
        {
            return (CommandTypeEnum)Enum.Parse(typeof(CommandTypeEnum), commandTypeAsString);
        }
    }
}
