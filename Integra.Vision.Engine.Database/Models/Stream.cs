//-----------------------------------------------------------------------
// <copyright file="Stream.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Stream class
    /// </summary>
    internal sealed class Stream : UserDefinedObject
    {
        /// <summary>
        /// Gets or sets the Duration time of the window
        /// </summary>
        public double DurationTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is join expression or not
        /// </summary>
        public bool UseJoin { get; set; }

        /// <summary>
        /// Gets or sets the condition list
        /// </summary>
        public ICollection<StreamCondition> Conditions { get; set; }

        /// <summary>
        /// Gets or sets the projection list
        /// </summary>
        public ICollection<PList> PList { get; set; }

        /// <summary>
        /// Gets or sets the trigger
        /// </summary>
        public ICollection<Trigger> Triggers { get; set; }

        /// <summary>
        /// Gets or sets the assigned sources
        /// </summary>
        public ICollection<SourceAssignedToStream> AsignedSources { get; set; }
    }
}
