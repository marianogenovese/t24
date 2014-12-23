//-----------------------------------------------------------------------
// <copyright file="SystemSetTrace.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models.SystemViews
{
    /// <summary>
    /// SystemSetTrace class
    /// </summary>
    internal sealed class SystemSetTrace
    {
        /// <summary>
        /// Gets or sets the trace level
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets the object id to trace
        /// </summary>
        public System.Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the trace
        /// </summary>
        public System.DateTime CreationDate { get; set; }
    }
}
