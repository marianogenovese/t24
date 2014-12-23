//-----------------------------------------------------------------------
// <copyright file="Trigger.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Trigger class
    /// </summary>
    internal sealed class Trigger : UserDefinedObject
    {
        /// <summary>
        /// Gets or sets the window duration
        /// </summary>
        public double DurationTime { get; set; }

        /// <summary>
        /// Gets or sets the associated stream
        /// </summary>
        public Stream Stream { get; set; }

        /// <summary>
        /// Gets or sets the stream id
        /// </summary>
        public System.Guid StreamId { get; set; }

        /// <summary>
        /// Gets or sets the statement list
        /// </summary>
        public ICollection<Stmt> Stmts { get; set; }
    }
}
