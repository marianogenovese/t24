//-----------------------------------------------------------------------
// <copyright file="Script.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models
{
    using System;

    /// <summary>
    /// Script model class
    /// </summary>
    internal sealed class Script
    {
        /// <summary>
        /// Gets or sets the script id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the script id
        /// </summary>
        public string ScriptText { get; set; }

        /// <summary>
        /// Gets or sets the date of the last modification
        /// </summary>
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// Gets or sets the user defined object
        /// </summary>
        public UserDefinedObject UserDefinedObject { get; set; }

        /// <summary>
        /// Gets or sets the user defined object id
        /// </summary>
        public Guid UserDefinedObjectId { get; set; }
    }
}
