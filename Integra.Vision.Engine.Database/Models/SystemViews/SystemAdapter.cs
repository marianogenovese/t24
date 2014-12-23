//-----------------------------------------------------------------------
// <copyright file="SystemAdapter.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models.SystemViews
{
    using System.Collections.Generic;

    /// <summary>
    /// Adapter class
    /// </summary>
    internal sealed class SystemAdapter
    {
        /// <summary>
        /// Gets or sets the adapter identifier
        /// </summary>
        public System.Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the adapter name
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

        /// <summary>
        /// Gets or sets the creation date of the adapter
        /// </summary>
        public System.DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets the adapter type
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets the adapter assembly reference
        /// </summary>
        public System.Guid Reference { get; set; }
    }
}
