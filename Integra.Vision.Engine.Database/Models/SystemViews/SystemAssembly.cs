//-----------------------------------------------------------------------
// <copyright file="SystemAssembly.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models.SystemViews
{
    /// <summary>
    /// SystemAssembly class
    /// </summary>
    internal sealed class SystemAssembly
    {
        /// <summary>
        /// Gets or sets the adapter id
        /// </summary>
        public System.Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the assembly local path
        /// </summary>
        public string LocalPath { get; set; }

        /// <summary>
        /// Gets or sets the assembly name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the assembly state
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is a system object
        /// </summary>
        public bool IsSystemObject { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the assembly
        /// </summary>
        public System.DateTime CreationDate { get; set; }
    }
}
