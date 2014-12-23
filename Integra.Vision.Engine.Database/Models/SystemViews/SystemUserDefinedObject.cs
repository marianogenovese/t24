//-----------------------------------------------------------------------
// <copyright file="SystemUserDefinedObject.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models.SystemViews
{
    /// <summary>
    /// SystemUserDefinedObject class
    /// </summary>
    internal sealed class SystemUserDefinedObject
    {
        /// <summary>
        /// Gets or sets the adapter identifier
        /// </summary>
        public System.Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the object name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the adapter state
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is a system object
        /// </summary>
        public bool IsSystemObject { get; set; }
    }
}
