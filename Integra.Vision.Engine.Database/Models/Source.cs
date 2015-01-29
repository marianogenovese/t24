//-----------------------------------------------------------------------
// <copyright file="Source.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Source class
    /// </summary>
    internal sealed class Source : UserDefinedObject
    {
        /// <summary>
        /// Gets or sets the source conditions
        /// </summary>
        public ICollection<SourceCondition> Conditions { get; set; }

        /// <summary>
        /// Gets or sets the assigned streams
        /// </summary>
        public ICollection<SourceAssignedToStream> AsignedStreams { get; set; }
    }
}
