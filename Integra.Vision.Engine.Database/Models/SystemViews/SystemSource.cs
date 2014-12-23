//-----------------------------------------------------------------------
// <copyright file="SystemSource.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models.SystemViews
{
    /// <summary>
    /// SystemSource class
    /// </summary>
    internal sealed class SystemSource
    {
        /// <summary>
        /// Gets or sets the adapter identifier
        /// </summary>
        public System.Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the adapter id
        /// </summary>
        public System.Guid AdapterId { get; set; }

        /// <summary>
        /// Gets or sets the source name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the source state
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is a system object
        /// </summary>
        public bool IsSystemObject { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the source
        /// </summary>
        public System.DateTime CreationDate { get; set; }
    }
}
