//-----------------------------------------------------------------------
// <copyright file="SetTrace.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models
{
    /// <summary>
    /// SetTrace class
    /// </summary>
    internal sealed class SetTrace
    {
        /// <summary>
        /// Gets or sets the trace level
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets the object to trace
        /// </summary>
        public UserDefinedObject UserDefinedObject { get; set; }

        /// <summary>
        /// Gets or sets the object id to trace
        /// </summary>
        public System.Guid UserDefinedObjectId { get; set; }

        /// <summary>
        /// Gets or sets the trace creation date
        /// </summary>
        public System.DateTime CreationDate { get; set; }
    }
}
