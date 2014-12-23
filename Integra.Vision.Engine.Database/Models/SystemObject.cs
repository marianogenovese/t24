//-----------------------------------------------------------------------
// <copyright file="SystemObject.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models
{
    /// <summary>
    /// Abstract class of the views
    /// </summary>
    internal abstract class SystemObject
    {
        /// <summary>
        /// Gets or sets the object id
        /// </summary>
        public System.Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the object creation date
        /// </summary>
        public System.DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets the object type
        /// </summary>
        public string Type { get; set; }
    }
}
